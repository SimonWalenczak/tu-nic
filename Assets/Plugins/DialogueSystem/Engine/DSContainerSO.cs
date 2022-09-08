using System.Collections.Generic;
using UnityEngine;

namespace DS.Engine {
    using Data;

    public class DSContainerSO : ScriptableObject {
        public List<DSNodeData> Nodes => nodes;
        public DSNodeData FirstNode => firstNode;

        [SerializeField] private List<DSNodeData> nodes;
        [SerializeField] private DSNodeData firstNode;

        public void Initialize(DSNodeData firstNode) {
            nodes = new List<DSNodeData>();
            this.firstNode = new DSNodeData(firstNode);
        }

        public void Add(DSNodeData data) => nodes.Add(new DSNodeData(data));
    }
}
