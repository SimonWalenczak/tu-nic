using System;
using UnityEngine;

namespace DS.Engine.Data {
    [Serializable]
    public class DSChoiceData {
        public string Name { get => name; set => name = value; }
        public string NextDialogueID { get => nextDialogueID; set => nextDialogueID = value; }

        [SerializeField] private string name;
        [SerializeField] private string nextDialogueID;

        public DSChoiceData(string name) {
            this.name = name;
            nextDialogueID = string.Empty;
        }
        public DSChoiceData(DSChoiceData data) {
            name = data.Name;
            nextDialogueID = data.NextDialogueID;
        }
    }
}
