using BlueGraph;
using UnityEngine;

namespace Ludole.Inventory
{
    [Node(Path = "Events", Deletable = false)]
    [Tags("Hidden")]
    [Output("Item", typeof(ItemBase), Multiple = false)]

    public class OnConstruct : EventNode
    {
        public ItemBase ItemSource;

        public override object OnRequestValue(Port port)
        {
            return ItemSource;
        }
    }

    [CreateAssetMenu(
        menuName = "Inventory/Tooltip Graph",
        fileName = "New TooltipGraph"
    )]
    [IncludeTags("Math", "Flow Control", "Tooltip")]
    public class TooltipGraph : Graph, IExecutes
    {
        public override string Title => "Tooltip";

        protected override void OnGraphEnable()
        {
#if UNITY_EDITOR
            if (GetNode<OnConstruct>() == null)
            {
                OnConstruct node = BlueGraph.Editor.NodeReflection.Instantiate<OnConstruct>();
                AddNode(node);
            }
#endif
        }

        public void Execute(IExecutableNode root, ExecutionFlowData data)
        {
            IExecutableNode next = root;
            int iterations = 0;
            while (next != null)
            {
                next = next.Execute(data);

                iterations++;
                if (iterations > 2000)
                {
                    Debug.LogError("Potential infinite loop detected. Stopping early.", this);
                    break;
                }
            }
        }
    }
}