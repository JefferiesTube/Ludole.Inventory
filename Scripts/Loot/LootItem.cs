using System.Collections.Generic;

namespace Ludole.Inventory
{
    public class LootItem : LootDataset
    {
        public ItemBase ItemTemplate;
        public int Amount;
        public override string Name => ItemTemplate != null ? ItemTemplate.Name : "No Item";

        public override void AssignLoot(List<LootEntry> loot)
        {
            loot.Add(new LootEntry(ItemTemplate, Amount));
        }
    }
}