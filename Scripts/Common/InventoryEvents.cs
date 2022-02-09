using System;
using UnityEngine.Events;

namespace Ludole.Inventory
{
    [Serializable]
    public class ItemSlotDisplayEvent : UnityEvent<ItemSlotDisplay>
    {
    }

    [Serializable]
    public class ItemOverflowEvent : UnityEvent<ItemSlot>
    {
    }

    [Serializable]
    public class InventoryChangedEvent : UnityEvent<InventoryBase>
    {
    }

    [Serializable]
    public class InventorySizeChangedEvent : UnityEvent<InventoryBase>
    {
    }
}