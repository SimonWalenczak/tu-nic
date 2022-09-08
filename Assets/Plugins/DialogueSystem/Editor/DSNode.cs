using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Editor {
    using Engine.Data;
    using Utility;

    public class DSNode : Node {
        public DSNodeData Data => (DSNodeData)userData;

        private DSGraphView graphView;

        public DSNode(DSGraphView graphView, DSNodeData data) {
            this.graphView = graphView;
            userData = new DSNodeData(data);
            title = Data.CharacterName;
            SetPosition(Data.Position);
        }

        #region Draw
        public void Draw() {
            DrawTitleContainer();
            DrawInputContainer();
            DrawOutputContainer();
            DrawExtensionContainer();

            RefreshExpandedState();
        }

        private void DrawTitleContainer() {
            Button addOutputButton = new Button(AddOutput) { text = "Add Output" };
            titleButtonContainer.Add(addOutputButton);

        }

        private void DrawInputContainer() => inputContainer.Add(this.CreatePort("Input", Direction.Input, Port.Capacity.Multi));

        private void DrawOutputContainer() {
            if(Data.Choices.Count == 0) {
                Port port = CreateOutput(new DSChoiceData("Next"), false);
                Data.Choices.Add(port.userData as DSChoiceData);
                outputContainer.Add(port);
                return;
            }
            bool first = true;
            foreach(DSChoiceData choice in Data.Choices.ToArray()) {
                outputContainer.Add(CreateOutput(choice, !first));
                first = false;
            }
        }

	    private void DrawExtensionContainer() {
		    extensionContainer.AddClasses("extension");
            extensionContainer.Add(ElementUtility.CreateTextArea(Data.CharacterName, "", ChangeCharacterName, "node__textfield", "node__character-textfield"));
            extensionContainer.Add(ElementUtility.CreateTextArea(Data.Text, "", ChangeText, "node__textfield", "node__dialogue-textfield"));
	        extensionContainer.Add(ElementUtility.CreateObjectField<Sprite>("Character Sprite", Data.CharacterSprite, false, ChangeCharacterSprite,"node__field"));
            extensionContainer.Add(ElementUtility.CreateObjectField<Sprite>("Background Sprite", Data.BackgroundSprite, false, ChangeBackgroundSprite,"node__field"));
            extensionContainer.Add(ElementUtility.CreateObjectField<Etienne.SoundSO>("Play Sound", Data.Sound, false, ChangeAudioClip,"node__field"));
        }

        private void ChangeCharacterName(ChangeEvent<string> evt) {
            Data.CharacterName = evt.newValue;
            title = evt.newValue;
            graphView.SetDirty();
        }

        private void ChangeText(ChangeEvent<string> evt) {
            Data.Text = evt.newValue;
            graphView.SetDirty();
        }

        private void ChangeCharacterSprite(ChangeEvent<Object> evt) {
            Data.CharacterSprite = evt.newValue as Sprite;
            graphView.SetDirty();
        }

        private void ChangeBackgroundSprite(ChangeEvent<Object> evt) {
            Data.BackgroundSprite = evt.newValue as Sprite;
            graphView.SetDirty();
        }

        private void ChangeAudioClip(ChangeEvent<Object> evt) {
            Data.Sound = evt.newValue as Etienne.SoundSO;
            graphView.SetDirty();
        }
        #endregion

        #region Utility
        public void SetPosition(Vector2 newPos) => base.SetPosition(new Rect(newPos, Vector2.zero));

        public override void SetPosition(Rect newPos) {
            Data.Position = newPos.position;
            base.SetPosition(newPos);
        }

        public void DisconnectAllPorts() {
            DisconnectInputPorts(null);
            DisconnectOutputPorts(null);
        }

        private void DisconnectInputPorts(DropdownMenuAction obj) => DisconnectPorts(inputContainer);

        private void DisconnectOutputPorts(DropdownMenuAction obj) => DisconnectPorts(outputContainer);

        private void DisconnectPorts(VisualElement container) {
            foreach(Port port in container.Children()) {
                if(port.connected) graphView.DeleteElements(port.connections);
            }
        }

        private void AddOutput() {
            Port port = CreateOutput(new DSChoiceData("New Choice"));
            Data.Choices.Add(port.userData as DSChoiceData);
            outputContainer.Add(port);
            graphView.SetDirty();
        }

        private Port CreateOutput(DSChoiceData data, bool deletable = true) {
            Port port = this.CreatePort();
            port.userData = data;

            port.contentContainer.Add(ElementUtility.CreateTextField(data.Name, null, evt => UpdatePortText(evt, port),
                "node__textfield", "node__textfield__hidden", "node__output-textfield"));
            if(deletable) port.contentContainer.Add(ElementUtility.CreateButton("X", () => RemovePort(port)));

            RefreshExpandedState();
            RefreshPorts();
            return port;
        }

        private void UpdatePortText(ChangeEvent<string> evt, Port port) {
            DSChoiceData data = port.userData as DSChoiceData;
            data.Name = evt.newValue;
            graphView.SetDirty();
        }

        private void RemovePort(Port port) {
            if(port.connected) {
                graphView.DeleteElements(port.connections);
            }
            Data.Choices.Remove(port.userData as DSChoiceData);
            outputContainer.Remove(port);
            graphView.SetDirty();
        }
        #endregion
    }
}
