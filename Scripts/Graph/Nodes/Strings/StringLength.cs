using GraphProcessor;
using UnityEngine;

namespace Ludole.Inventory
{
    [NodeMenuItem("Strings/Length")]
    public class StringLengthNode : DataNode
    {
        [Input, SerializeField] public string Text;
        [Output] public int Length;

        public override string name => "Length (String)";

        protected override void Process()
        {
            base.Process();
            Length = Text.Length;
        }
    }
}