using System.Collections.Generic;

namespace Ludole.Inventory
{
    public class LootLink : LootDataset
    {
        public LootTable LootTable;

        public override void AssignLoot(List<LootEntry> loot)
        {
            loot.AddRange(LootTable.GetLoot());
        }
    }
}