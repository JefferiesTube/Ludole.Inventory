using GraphProcessor;
using UnityEngine;

namespace Ludole.Inventory
{
    [NodeMenuItem("Arithmetic/Integer/int > int")]
    public class IntegerCompareGreater : MathNode
    {
        public override string name => "A > B";

        [Input, SerializeField] public int A;
        [Input, SerializeField] public int B;

        [Output] public bool Result;
        protected override void Process()
        {
            base.Process();
            Result = A > B;
        }
    }
}