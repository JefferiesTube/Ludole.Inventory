using UnityEngine;
using BlueGraph;

namespace Ludole.Inventory
{
    [Node(Path = "Math/Boolean")]
    [Tags("Math")]
    public class And : MathNode<bool, bool, bool>
    {
        public override bool Execute(bool value1, bool value2)
        {
            return value1 && value2;
        }
    }
}
