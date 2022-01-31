
using UnityEngine;
using BlueGraph;

namespace Ludole.Inventory
{
    [Node(Path = "Math/Operation")]
    [Tags("Math")]
    public class Sign : MathNode<float, float>
    {
        public override float Execute(float value)
        {
            return Mathf.Sign(value);
        }
    }
}
