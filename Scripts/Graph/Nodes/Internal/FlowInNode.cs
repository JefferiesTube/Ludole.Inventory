using System;
using System.Collections.Generic;
using System.Reflection;
using GraphProcessor;

namespace Ludole.Inventory
{
    public abstract class FlowInNode : BaseNode, ITooltipFactoryFlow
    {
        [Input(name = "Flow")] 
        public TooltipFactoryLink FlowIn;

        public override string layoutStyle => "FlowNode";

        public abstract IEnumerable<BaseNode> GetExecutedNodes();

        public override FieldInfo[] GetNodeFields()
        {
            FieldInfo[] fields = base.GetNodeFields();
            Array.Sort(fields, (f1, f2) => f1.Name == nameof(FlowIn) ? -1 : 1);
            return fields;
        }
    }
}