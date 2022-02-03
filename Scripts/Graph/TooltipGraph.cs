using System.Linq;
using Ludole.Core;
using UnityEditor;
using UnityEngine;

namespace Ludole.Inventory
{
    [CreateAssetMenu(fileName = "tooltipGraph.asset", menuName = "Inventory/Graphs/Tooltip")]
    public class TooltipGraph : LudoleGraph
    {
        [HideInInspector] public GameObject TooltipRoot;
        [HideInInspector] public ItemBase Item;

        protected override void OnEnable()
        {
            if (!nodes.OfType<TooltipConstructEventNode>().Any())
            {
                TooltipConstructEventNode newNode = new TooltipConstructEventNode
                {
                    GUID = GUID.Generate().ToString()
                };
                nodes.Add(newNode);
            }

            base.OnEnable();
        }
    }
}