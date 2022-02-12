using System;
using System.Collections.Generic;
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
    public class InventoryChangedEvent : UnityEvent
    {
    }

    [Serializable]
    public class InventorySizeChangedEvent : UnityEvent<InventoryBase>
    {
    }

    [Serializable]
    public class ItemEvent : UnityEvent<ItemBase>
    {
    }

    [Serializable]
    public class InventorySelectionEvent : UnityEvent<IList<int>>
    {
    }
}