using System.Collections.Generic;

namespace Ludole.Inventory
{
    public class LootLink : LootDataset
    {
        public LootTable LootTable;

        public override string Name => LootTable != null ? LootTable.name : "No Link";

        public override void AssignLoot(List<LootEntry> loot)
        {
            loot.AddRange(LootTable.GetLoot());
        }
    }
}