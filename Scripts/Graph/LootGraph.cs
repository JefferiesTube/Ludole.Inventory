using System.Linq;
using Ludole.Core;
using UnityEditor;
using UnityEngine;

namespace Ludole.Inventory
{
    [CreateAssetMenu(fileName = "lootGraph.asset", menuName = "Inventory/Graphs/Loot")]
    public class LootGraph : LudoleGraph
    {
        [HideInInspector] public GameObject TooltipRoot;
        [HideInInspector] public ItemBase Item;

        protected override void OnEnable()
        {
            if (!nodes.OfType<LootConstructEventNode>().Any())
            {
                LootConstructEventNode newNode = new LootConstructEventNode
                {
                    GUID = GUID.Generate().ToString()
                };
                nodes.Add(newNode);
            }

            base.OnEnable();
        }
    }
}