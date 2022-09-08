using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Editor {
    using Engine.Data;
    using Utility;

    public class DSGraphView : GraphView {
        public new Dictionary<string, DSNode> nodes { get; set; }
        public bool IsSaveable => firstNodeID != string.Empty;
        public bool IsDirty => isDirty;
        public DSNodeData FirstNode => nodes[firstNodeID].Data;

        private bool isDirty;
        private string firstNodeID;
        private DSWindow window;
        private DSStartNode startNode;

        public DSGraphView(DSWindow window) {
            this.window = window;
            nodes = new Dictionary<string, DSNode>();
            startNode = new DSStartNode();
            AddElement(startNode);
            firstNodeID = string.Empty;

            AddManipulators();
            AddGridBackground();
            AddStyle();

            deleteSelection = DeleteSelectionOverride;
            //serializeGraphElements
            //unserializeAndPaste
            graphViewChanged = OnGraphViewChanged;
        }

        #region Constructor
        #region Manipulators
        private void AddManipulators() {
            SetupZoom(ContentZoomer.DefaultMinScale / 2, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());
            this.AddManipulator(CreateNodeContextualMenu);
        }

        private IManipulator CreateNodeContextualMenu => new ContextualMenuManipulator(
                evt => evt.menu.InsertAction(0, "Add Dialogue", CreateAndAddNewNode)
                );

        public void SetFirstNode(DSNode firstNode) {
            Port outputStart = startNode.outputContainer.Children().First() as Port;
            Port inputStart = firstNode.inputContainer.Children().First() as Port;
            AddElement(outputStart.ConnectTo(inputStart));
            startNode.RefreshPorts();
            firstNodeID = firstNode.Data.Id;
        }
        #endregion

        private void AddGridBackground() {
            GridBackground grid = new GridBackground();
            grid.StretchToParentSize();
            Insert(0, grid);
        }

        private void AddStyle() => this.AddStyleSheets("DSGraph", "DSNode");
        #endregion

        #region Override
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
            List<Port> compatiblePorts = new List<Port>();
            ports.ForEach(port => {
                if(startPort != port && startPort.node != port.node && startPort.direction != port.direction) {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }

        private void DeleteSelectionOverride(string operationName, AskUser askUser) {
            List<Edge> edgesToDelete = new List<Edge>();
            List<DSNode> nodesToDelete = new List<DSNode>();
            foreach(ISelectable element in selection) {
                if(element is DSNode node) nodesToDelete.Add(node);
                else if(element is Edge edge) edgesToDelete.Add(edge);
            }
            foreach(DSNode node in nodesToDelete) RemoveNode(node);
            DeleteElements(edgesToDelete);
        }

        //todo : Decompose into Methods
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange) {
            if(graphViewChange.edgesToCreate != null) {
                foreach(Edge edge in graphViewChange.edgesToCreate) {
                    if(edge.output.node is DSStartNode startNode) {
                        DSNode node = edge.input.node as DSNode;
                        firstNodeID = node.Data.Id;
                        window.CheckDirty();
                    } else if(edge.input.node is DSNode node) {
                        DSChoiceData outputData = edge.output.userData as DSChoiceData;
                        outputData.NextDialogueID = node.Data.Id;
                    }
                }
            }
            if(graphViewChange.elementsToRemove != null) {
                foreach(GraphElement element in graphViewChange.elementsToRemove) {
                    if(element is Edge edge) {
                        if(edge.output.node is DSStartNode) {
                            firstNodeID = string.Empty;
                            window.CheckDirty();
                        } else if(edge.output.node is DSNode) {
                            DSChoiceData outputData = edge.output.userData as DSChoiceData;
                            outputData.NextDialogueID = string.Empty;
                        }
                    }
                }
            }
            SetDirty();
            return new GraphViewChange();
        }
        #endregion

        #region Creation
        private void CreateAndAddNewNode(DropdownMenuAction obj) {
            DSNode node = CreateNode(new DSNodeData() { Position = GetLocalMousePosition(obj.eventInfo.localMousePosition) });
            node.Draw();
            AddNode(node);
        }

        public DSNode CreateAndAddNode(DSNodeData data) => AddNode(CreateNode(data));

        private DSNode CreateNode(DSNodeData data) => new DSNode(this, data);

        private DSNode AddNode(DSNode node) {
            nodes.Add(node.Data.Id, node);
            AddElement(node);
            SetDirty();
            return node;
        }
        #endregion

        #region Destruction
        private void RemoveNode(DSNode node) {
            node.DisconnectAllPorts();
            nodes.Remove(node.Data.Id);
            RemoveElement(node);
        }
        #endregion

        #region Utility
        public Vector2 GetLocalMousePosition(Vector2 mousePosition) {
            Vector2 worldMousePosition = mousePosition;
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            return localMousePosition;
        }
        public void ClearGraph() {
            graphElements.ForEach(RemoveElement);
            nodes.Clear();
            startNode = new DSStartNode();
            AddElement(startNode);
            SetDirty();
        }

        public void SetDirty() {
            isDirty = true;
            window.CheckDirty();
        }

        public void Clean() {
            isDirty = false;
            window.CheckDirty();
        }
        #endregion
    }
}
