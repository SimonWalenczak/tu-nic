using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DS.Editor.Utility {
    public static class ElementUtility {
        public static Button CreateButton(string text, System.Action onClick = null, params string[] classNames) {
            Button button = new Button(onClick) { text = text };
            button.AddClasses(classNames);
            return button;
        }

        public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null, params string[] classNames) {
            TextField textfield = new TextField() { value = value, label = label };
            if(onValueChanged != null) textfield.RegisterValueChangedCallback(onValueChanged);
            textfield.AddClasses(classNames);
            return textfield;
        }

        public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null, params string[] classNames) {
            TextField textfield = CreateTextField(value, label, onValueChanged);
            textfield.multiline = true;
            textfield.AddClasses(classNames);
            return textfield;
        }

        public static Port CreatePort(this Node node, string portName = "", Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single, Orientation orientation = Orientation.Horizontal) {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
            port.portName = portName;
            return port;
        }

        public static ObjectField CreateObjectField<T>(string label = "", UnityEngine.Object value = null, bool allowSceneObjects = false, EventCallback<ChangeEvent<UnityEngine.Object>> onValueChanged = null, params string[] classNames) {
            ObjectField objectField = new ObjectField(label) {
                objectType = typeof(T),
                allowSceneObjects = allowSceneObjects,
                value = value,
            };
            if(onValueChanged != null) objectField.RegisterValueChangedCallback(onValueChanged);
            objectField.AddClasses(classNames);
            return objectField;
        }
    }
}