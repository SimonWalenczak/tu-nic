using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Editor {
    using Utility;
    public class DSStartNode : Node {
        public Node StartingNode => output.connected ? output.connections.First().input.node : null;

        private Port output;
        public DSStartNode() {
            title = "Start";
            output = this.CreatePort("");
            outputContainer.Add(output);
            titleContainer.style.backgroundColor = new Color(169 / 255f, 43 / 255f, 31 / 255f);
            SetPosition(new Rect(new Vector2(50, 150), Vector2.zero));
            RefreshExpandedState();
        }
    }
}
