using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Editor {
    using Engine;
    using Engine.Data;
    using UnityEditor.Callbacks;
    using Utility;

    public class DSWindow : EditorWindow {
        private static DSGraphView graphView;
        private static DSContainerSO container;
        private ObjectField InspectedObject;
        public Button saveButton, loadButton, clearButton;

        [MenuItem("Window/Dialogue Graph")]
        public static DSWindow Open() {
            DSWindow window = GetWindow<DSWindow>("Dialogue Graph");
            window.InspectedObject.value = container;
            window.Load();
            return window;
        }

        public static DSWindow Open(DSContainerSO container) {
            DSWindow.container = container;
            DSWindow window = Open();
            return window;
        }

        [OnOpenAsset()]
        public static bool OnOpenAsset(int instanceID, int line) {
            Object obj = EditorUtility.InstanceIDToObject(instanceID);
            if(obj is DSContainerSO container) {
                _ = Open(container);
                return true;
            }
            return false;
        }

        private void OnEnable() {
            AddGraphView();
            AddToolBar();
        }

        //private void OnDestroy() => AskToSave();

        private void AddGraphView() {
            graphView = new DSGraphView(this);
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void AddToolBar() {
            Toolbar toolbar = new Toolbar();
            InspectedObject = ElementUtility.CreateObjectField<DSContainerSO>("", container, false, CheckButtons);
            Button saveAsButton = ElementUtility.CreateButton("Save As", SaveAs);
            saveButton = ElementUtility.CreateButton("Save", Save);
            loadButton = ElementUtility.CreateButton("Load", Load);
            clearButton = ElementUtility.CreateButton("Clear", Clear);

            toolbar.Add(InspectedObject);
            toolbar.Add(saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(saveAsButton);
            toolbar.Add(clearButton);

            CheckButtons(null);

            rootVisualElement.Add(toolbar);
        }

        public void CheckDirty() {
            bool isNull = InspectedObject == null || InspectedObject.value == null;
            saveButton.SetEnabled(!isNull && graphView.IsSaveable && graphView.IsDirty);
            loadButton.SetEnabled(!isNull);
        }

        private void CheckButtons(ChangeEvent<Object> evt) => Load();

        private void AskToSave() {
            if(!graphView.IsDirty) return;

            if(EditorUtility.DisplayDialog("Graph Has Been Modified",
                "Do you want to save the changes you made in the graph:\n" +
                $"{InspectedObject.value}\n\n" +
                "Your changes will be lost if you don't save them.",
                "Save", "Don't Save")) {
                Save();
            }
        }

        private void SaveAs() {
            if(!graphView.IsSaveable) {
                EditorUtility.DisplayDialog("Start Not Connected", "The Start node is not connected, \n" +
                    "make sure to connect it", "Understood !");
                return;
            }
            string globalPath = EditorUtility.SaveFilePanel("Dialogue System Graphs", "Assets", "DialogueContainer", "asset");
            string path = IOUtility.GlobalPathToRelativePath(globalPath);
            if(string.IsNullOrEmpty(path)) return;

            string filename = Path.GetFileNameWithoutExtension(path);
            path = IOUtility.GetParentPath(path);
            DSContainerSO dialogueContainer = IOUtility.CreateAsset<DSContainerSO>(path, filename);
            SaveGraph(dialogueContainer);
        }

        private void Save() => SaveGraph(container);

        //todo :Deecompose into methodes
        private void SaveGraph(DSContainerSO dialogueContainer) {
            if(!graphView.IsSaveable) {
                EditorUtility.DisplayDialog("Start Not Connected", "The Start node is not connected, \n" +
                    "make sure to connect it", "Understood !");
                return;
            }
            dialogueContainer.Initialize(graphView.FirstNode);
            foreach(KeyValuePair<string, DSNode> node in graphView.nodes) dialogueContainer.Add(node.Value.Data);
            IOUtility.SaveAsset(dialogueContainer);
            InspectedObject.value = dialogueContainer;
            graphView.Clean();
        }

        //todo :Deecompose into methodes
        private void Load() {
            graphView.ClearGraph();
            container = InspectedObject.value as DSContainerSO;
            if(container == null) return;

            Dictionary<string, DSNode> loadedNodes = new Dictionary<string, DSNode>();
            foreach(DSNodeData data in container.Nodes) {
                DSNode node = graphView.CreateAndAddNode(data);
                node.Draw();
                loadedNodes.Add(node.Data.Id, node);
            }

            graphView.SetFirstNode(loadedNodes[container.FirstNode.Id]);

            foreach(KeyValuePair<string, DSNode> node in loadedNodes) {
                foreach(Port port in node.Value.outputContainer.Children()) {
                    DSChoiceData data = port.userData as DSChoiceData;
                    if(data.NextDialogueID.Equals(string.Empty)) continue;

                    DSNode nextNode = loadedNodes[data.NextDialogueID];
                    Port nextInput = nextNode.inputContainer.Children().First() as Port;
                    Edge edge = port.ConnectTo(nextInput);
                    graphView.AddElement(edge);
                }
                node.Value.RefreshPorts();
            }
            graphView.Clean();
        }

        private void Clear() => graphView.ClearGraph();
    }
}