using GraphProcessor;
using UnityEngine;

namespace Ludole.Inventory
{
    [CreateAssetMenu(fileName = "tooltipGraph.asset", menuName = "Inventory/Graphs/Tooltip")]
    public class TooltipFactoryGraph : BaseGraph
    {
        [HideInInspector] public GameObject TooltipRoot;
        [HideInInspector] public ItemBase Item;
    }
}