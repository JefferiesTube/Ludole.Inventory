using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;

namespace Ludole.Inventory
{
    [Serializable]
    public abstract class FlowOutNode : BaseNode, ITooltipFactoryFlow
    {
        [Output(name = "Flow")] public TooltipFactoryLink FlowOut;

        public override string layoutStyle => "FlowNode";

        public IEnumerable<BaseNode> GetExecutedNodes()
        {
            return outputPorts.FirstOrDefault(n => n.fieldName == nameof(FlowOut))
                ?.GetEdges().Select(e => e.inputNode);
        }

        public override FieldInfo[] GetNodeFields()
        {
            FieldInfo[] fields = base.GetNodeFields();
            Array.Sort(fields, (f1, f2) => f1.Name == nameof(FlowOut) ? -1 : 1);
            return fields;
        }
    }
}