using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludole.Inventory
{
    [CreateAssetMenu(fileName = "New Loot Table.asset", menuName = "Inventory/Loot Table")]
    public class LootTable : ScriptableObject
    {
        [SerializeReference] public LootDataset Root;
        public bool Empty => Root == null;

        public List<LootEntry> GetLoot()
        {
            List<LootEntry> lootData = new List<LootEntry>();
            Root.AssignLoot(lootData);
            return lootData;
        }
    }
}
