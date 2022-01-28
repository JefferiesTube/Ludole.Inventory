using System;
using System.Collections.Generic;
using MarkupAttributes;
using UnityEngine;

namespace Ludole.Inventory
{
    [Serializable]
    public class ItemSlot
    {
        [ReadOnly] public ItemBase Content;
        public bool IsTemporary;
        public bool IsEmpty => Content == null;

        public bool FixedItemType;
        [ShowIf(nameof(FixedItemType), true)] public ItemBase FixedItemTemplate;

        public bool OverrideStackLimit;
        [ShowIf(nameof(OverrideStackLimit), true), Min(0)] public int OverriddenStackLimit;

        public ConstraintMode ConstraintMode;
        public List<Category> SlotConstraints;
    }
}