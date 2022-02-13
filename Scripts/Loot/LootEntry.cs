using System;

namespace Ludole.Inventory
{
    [Serializable]
    public class LootEntry
    {
        public ItemBase ItemTemplate;
        public int Amount;

        public LootEntry(ItemBase itemTemplate, int amount)
        {
            ItemTemplate = itemTemplate;
            Amount = amount;
        }
    }
}