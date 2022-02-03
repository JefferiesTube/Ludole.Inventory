using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using UnityEngine;

namespace Ludole.Inventory
{
    [NodeMenuItem("Flow Control/Branch")]
    public class BranchNode : FlowInNode
    {
        [Input(name = "Condition"), SerializeField] public bool condition;

        [Output(name = "True")] public TooltipFactoryLink @true;
        [Output(name = "False")] public TooltipFactoryLink @false;

        public override string name => "Branch";

        public override IEnumerable<BaseNode> GetExecutedNodes()
        {
            string fieldName = condition ? nameof(@true) : nameof(@false);

            var x = outputPorts.FirstOrDefault(n => n.fieldName == fieldName)
                ?.GetEdges().Select(e => e.inputNode);
            return x;
        }
    }
}