using System;
using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using UnityEngine;

namespace Ludole.Inventory
{
    [Serializable, NodeMenuItem("Flow Control/Sequence")]
    public class SequenceNode : FlowInNode
    {
        [Output(name = "First")]
        public TooltipFactoryLink LoopBody;

        [Output(name = "Second")]
        public TooltipFactoryLink LoopCompleted;

        [Input, SerializeField] public int Start = 0;
        [Input, SerializeField] public int End = 10;

        [Output] public int Index;

        public override string name => "ForLoop";

        protected override void Process() => Index++;

        public override IEnumerable<BaseNode> GetExecutedNodes() => throw new Exception("Do not use GetExecutedNoes in for loop to get it's dependencies");

        public IEnumerable<BaseNode> GetExecutedNodesLoopBody()
        {
            return outputPorts.FirstOrDefault(n => n.fieldName == nameof(LoopBody))
                ?.GetEdges().Select(e => e.inputNode);
        }

        public IEnumerable<BaseNode> GetExecutedNodesLoopCompleted()
        {
            return outputPorts.FirstOrDefault(n => n.fieldName == nameof(LoopCompleted))
                ?.GetEdges().Select(e => e.inputNode);
        }
    }
}