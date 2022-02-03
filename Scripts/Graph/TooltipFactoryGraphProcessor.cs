using System;
using GraphProcessor;
using System.Collections.Generic;
using System.Linq;

namespace Ludole.Inventory
{
    public class TooltipFactoryGraphProcessor : BaseGraphProcessor
    {
        private TooltipConstructEventNode m_StartEventNode;

        public TooltipFactoryGraphProcessor(BaseGraph graph) : base(graph)
        {
        }

        public override void UpdateComputeOrder()
        {
            m_StartEventNode = (TooltipConstructEventNode) graph.nodes.First(n => n is TooltipConstructEventNode);
        }

        public override void Run()
        {
            if (m_StartEventNode != null)
            {
                Stack<BaseNode> nodeStack = new Stack<BaseNode>();
                nodeStack.Push(m_StartEventNode);
                RunGraph(nodeStack);
            }
        }

        private void RunGraph(Stack<BaseNode> nodeStack)
        {
            HashSet<BaseNode> nodeDependenciesGathered = new HashSet<BaseNode>();
            HashSet<BaseNode> skipConditionalHandling = new HashSet<BaseNode>();

            while (nodeStack.Count > 0)
            {
                BaseNode node = nodeStack.Pop();

                if (skipConditionalHandling.Contains(node))
                {
                    node.OnProcess();
                    continue;
                }

                switch (node)
                {
                    case FlowOutNode flowOutNode:
                        ProcessFlowNode(nodeStack, nodeDependenciesGathered, node, flowOutNode);
                        break;
                    case ForLoopNode forLoopNode:
                        forLoopNode.Index = forLoopNode.Start - 1; // Initialize the start index
                        foreach (FlowOutNode n in forLoopNode.GetExecutedNodesLoopCompleted())
                            nodeStack.Push(n);
                        for (int i = forLoopNode.Start; i < forLoopNode.End; i++)
                        {
                            foreach (FlowOutNode n in forLoopNode.GetExecutedNodesLoopBody())
                                nodeStack.Push(n);

                            nodeStack.Push(node);
                        }

                        skipConditionalHandling.Add(node);
                        break;
                    case BranchNode bf:
                        ProcessBranchNode(nodeStack, nodeDependenciesGathered, skipConditionalHandling, bf);
                        break;
                    case FlowInNode flowIn:
                        ProcessFlowNode(nodeStack, nodeDependenciesGathered, node, flowIn);
                        break;
                    default:
                        node.OnProcess();
                        break;
                }
            }
        }

        private void ProcessBranchNode(Stack<BaseNode> nodeStack, HashSet<BaseNode> nodeDependenciesGathered,
            HashSet<BaseNode> skipConditionalHandling, BranchNode node)
        {
            if (nodeDependenciesGathered.Contains(node))
            {
                node.OnProcess();
                foreach (BaseNode n in node.GetExecutedNodes())
                    nodeStack.Push(n);

                nodeDependenciesGathered.Remove(node);
                skipConditionalHandling.Add(node);
            }
            else
            {
                nodeStack.Push(node);
                nodeDependenciesGathered.Add(node);
                foreach (BaseNode nonConditionalNode in GatherNonConditionalDependencies(node))
                {
                    nodeStack.Push(nonConditionalNode);
                }
            }
        }

        private void ProcessFlowNode(Stack<BaseNode> nodeStack, HashSet<BaseNode> nodeDependenciesGathered, BaseNode node, ITooltipFactoryFlow tfn)
        {
            if (nodeDependenciesGathered.Contains(node))
            {
                node.OnProcess();
                foreach (BaseNode n in tfn.GetExecutedNodes())
                    nodeStack.Push(n);

                nodeDependenciesGathered.Remove(node);
            }
            else
            {
                nodeStack.Push(node);
                nodeDependenciesGathered.Add(node);
                foreach (BaseNode nonConditionalNode in GatherNonConditionalDependencies(node))
                {
                    nodeStack.Push(nonConditionalNode);
                }
            }
        }

        private IEnumerable<BaseNode> GatherNonConditionalDependencies(BaseNode node)
        {
            Stack<BaseNode> dependencies = new Stack<BaseNode>();
            dependencies.Push(node);

            while (dependencies.Count > 0)
            {
                BaseNode dependency = dependencies.Pop();

                foreach (BaseNode d in dependency.GetInputNodes().Where(n => n is not FlowOutNode))
                    dependencies.Push(d);

                if (dependency != node)
                    yield return dependency;
            }
        }
    }
}