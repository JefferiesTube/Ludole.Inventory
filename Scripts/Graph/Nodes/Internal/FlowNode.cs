using System;
using System.Reflection;
using GraphProcessor;

namespace Ludole.Inventory
{
    public abstract class FlowNode : FlowOutNode
    {
        [Input(name = "Flow", allowMultiple = true)]
        public TooltipFactoryLink FlowIn;

        public override FieldInfo[] GetNodeFields()
        {
            FieldInfo[] fields = base.GetNodeFields();
            Array.Sort(fields, (f1, f2) => f1.Name == nameof(FlowIn) || f1.Name == nameof(FlowOut) ? -1 : 1);
            return fields;
        }
    }
}