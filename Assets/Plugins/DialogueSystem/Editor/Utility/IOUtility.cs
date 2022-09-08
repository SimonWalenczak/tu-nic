using UnityEditor;
using UnityEngine;

namespace DS.Editor.Utility {
    public static class IOUtility {
        #region Utility
        public static string GlobalPathToRelativePath(string globalPath) {
            const string localStart = "Assets";
            int index = -1;
            string found = "";

            for(int i = 0; i < globalPath.Length; i++) {
                char c = globalPath[i];

                if(c == localStart[found.Length]) {
                    found += c;
                    if(found.Length == 1) index = i;
                    if(found.Length == localStart.Length) {
                        return globalPath.Substring(index);
                    }
                } else {
                    found = "";
                    index = -1;
                }
            }
            return null;
        }

        public static string GetParentPath(string path) => path.Substring(0, path.LastIndexOf('/'));

        public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject {
            string fullPath = $"{path}/{assetName}.asset";
            T asset = LoadAsset<T>(path, assetName);
            if(asset != null) return asset;
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, fullPath);
            return asset;
        }

        public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject => AssetDatabase.LoadAssetAtPath<T>($"{path}/{assetName}.asset");

        public static void RemoveAsset(string path, string assetName) => AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");

        public static void SaveAsset(Object obj) {
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
        }
        #endregion
    }
}
