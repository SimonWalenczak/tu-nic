using UnityEditor;
using UnityEngine.UIElements;

namespace DS.Editor.Utility {
    public static class StyleUtility {
        public static VisualElement AddClasses(this VisualElement element, params string[] classNames) {
            foreach(string className in classNames)
                element.AddToClassList(className);
            return element;
        }

        public static VisualElement AddStyleSheets(this VisualElement element, params string[] styleSheetsName) {
            foreach(string styleSheetName in styleSheetsName)
	            element.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>($"Assets/Plugins/DialogueSystem/Editor/Styles/{styleSheetName}.uss"));
            return element;
        }
    }
}