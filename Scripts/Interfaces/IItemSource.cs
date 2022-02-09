using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludole.Inventory
{
    public interface IItemSource
    {
        Transform GetDragDropRootTransform { get; }
        void Enable();
        void Disable();
        void ToggleRaycastTarget(bool newState);
        GameObject VisualSource { get; }
        InventoryBase Inventory { get; }
        int Index { get; set; }
        bool IsFree(int index, ItemBase item);
        ItemBase GetItem();
        bool PassesInventorySpecificCheck(ItemBase item, int index);
        RectTransform VisualTransform { get; }
    }
}
