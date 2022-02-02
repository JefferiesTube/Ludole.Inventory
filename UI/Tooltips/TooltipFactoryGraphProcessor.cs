using GraphProcessor;
using System.Collections.Generic;
using System.Linq;

namespace Ludole.Inventory
{
    public class TooltipFactoryGraphProcessor : BaseGraphProcessor
    {
        private FlowOutStartNode _startNode;

        public TooltipFactoryGraphProcessor(BaseGraph graph) : base(graph)
        {
        }

        public override void UpdateComputeOrder()
        {
            _startNode = (FlowOutStartNode) graph.nodes.First(n => n is FlowOutStartNode);
        }

        public override void Run()
        {
            if (_startNode != null)
            {
                Stack<BaseNode> nodeStack = new Stack<BaseNode>();
                nodeStack.Push(_startNode);
                RunGraph(nodeStack);
            }
        }

        private void RunGraph(Stack<BaseNode> nodeStack)
        {
            HashSet<BaseNode> nodeDependenciesGathered = new HashSet<BaseNode>();

            while (nodeStack.Count > 0)
            {
                BaseNode node = nodeStack.Pop();

                if (node is FlowOutNode tfn)
                {
                    if (nodeDependenciesGathered.Contains(node))
                    {
                        node.OnProcess();
                        foreach (FlowOutNode n in tfn.GetExecutedNodes())
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
                else
                {
                    node.OnProcess();
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