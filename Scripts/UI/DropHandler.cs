using Ludole.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ludole.Inventory
{
    public class DropHandler : MonoBehaviour, IDropHandler
    {
        private ItemSlotDisplay Target;
        private ItemSlotDisplay Source;

        private bool _sourceChanged;
        private bool _targetChanged;
        [HideInInspector] public bool _isSplitOperation;
        

        protected virtual void Start()
        {
            Target = GetComponentInParent<ItemSlotDisplay>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            try
            {
                _sourceChanged = false;
                _targetChanged = false;

                Source = Manager.Use<DragDropHandler>().DragSource;
                _isSplitOperation = Manager.Use<DragDropHandler>().IsSplitOperation;

                if (OperateOnSameSlot())
                    return;

                if (TargetSlotIsFree())
                {
                    if (!TargetInventorySuitable())
                        return;
                    MoveItem();
                }
                else
                {
                    if (!SwapSuitable())
                        return;
                    SwapItems();
                }
            }
            finally
            {
                if (_sourceChanged)
                    Source.Inventory.Changed();
                if (_targetChanged)
                    Target.Inventory.Changed();
                Manager.Use<DragDropHandler>().RestoreDragOperation();
            }
        }

        private bool OperateOnSameSlot() => !UsesDifferentInventories() && Source.SlotIndex == Target.SlotIndex;

        private bool UsesDifferentInventories() => Source.Inventory != Target.Inventory;

        private bool TargetSlotIsFree() => Target.Inventory[Target.SlotIndex].IsEmpty;

        private void MoveItem()
        {
            CreateItemAtTarget();
            if (_isSplitOperation && Source.GetItem() is IStackable stackA && stackA.StackSize >= 2)
            {
                int originalStackSize = stackA.StackSize;
                stackA.StackSize = Mathf.FloorToInt(originalStackSize / 2.0f);
                ((IStackable) Target.GetItem()).StackSize = Mathf.CeilToInt(originalStackSize / 2.0f);
                _sourceChanged = true;
                _targetChanged = true;
            }
            else
            {
                RemoveSourceItem();
            }
        }

        private void CreateItemAtTarget()
        {
            Target.Inventory[Target.SlotIndex].Content = Instantiate(Source.GetItem());
            // TODO: Stacking
            // TODO: TrashSlot
            _targetChanged = true;
        }

        private void RemoveSourceItem()
        {
            Source.Inventory.ClearSlot(Source.SlotIndex);
            _sourceChanged = true;
        }

        private void SwapItems()
        {
            if (CanStack(Source.Inventory, Source.SlotIndex, Target.Inventory, Target.SlotIndex, out int amount))
            {
                IStackable stackA = (IStackable)Source.GetItem();
                IStackable stackB = (IStackable)Target.GetItem();

                if (_isSplitOperation)
                {
                    amount = Mathf.Min(amount, Mathf.CeilToInt(stackA.StackSize / 2.0f));
                }

                stackB.StackSize += amount;
                stackA.StackSize -= amount;
                if(stackA.StackSize == 0)
                    RemoveSourceItem();

                _targetChanged = true;
                _sourceChanged = true;
            }
            else
            {
                ItemBase tmpItem = Target.GetItem();
                Target.Inventory[Target.SlotIndex].Content = Source.GetItem();
                Source.Inventory[Source.SlotIndex].Content = tmpItem;
                _targetChanged = true;
                _sourceChanged = true;
            }
        }

        private static bool DoConstraintCheck(ItemBase item, ConstraintMode mode, List<Category> constraints)
        {
            if (constraints == null || constraints.Count == 0)
                return true;

            return mode switch
            {
                ConstraintMode.Any => constraints.Intersect(item.Categories).Any(),
                ConstraintMode.All => constraints.Intersect(item.Categories).Count() == constraints.Count,
                ConstraintMode.Invert => !constraints.Intersect(item.Categories).Any(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private bool PassesConstraintCheck(ItemBase item, Inventory inventory)
        {
            return DoConstraintCheck(item, inventory.ConstraintMode, inventory.Constraints);
        }

        private bool PassesSlotCheck(ItemBase item, Inventory inventory, int slotIndex)
        {
            if (inventory[slotIndex].FixedItemType && !item.IsSame(inventory[slotIndex].FixedItemTemplate))
                return false;

            return DoConstraintCheck(item, inventory[slotIndex].ConstraintMode, inventory[slotIndex].SlotConstraints);
        }

        private bool TargetInventorySuitable() => PassesConstraintCheck(Source.GetItem(), Target.Inventory)
                                                  && PassesSlotCheck(Source.GetItem(), Target.Inventory, Target.SlotIndex);

        private bool SwapSuitable() =>
            PassesConstraintCheck(Source.GetItem(), Target.Inventory)
            && PassesConstraintCheck(Target.GetItem(), Source.Inventory)
            && PassesSlotCheck(Source.GetItem(), Target.Inventory, Target.SlotIndex)
            && PassesSlotCheck(Target.GetItem(), Source.Inventory, Source.SlotIndex);

        private bool CanStack(Inventory invA, int slotA, Inventory invB, int slotB, out int stackableAmount)
        {
            stackableAmount = 0;
            ItemBase itemA = invA[slotA].Content;
            ItemBase itemB = invB[slotB].Content;

            if (itemA is not IStackable stackA)
                return false;
            if (itemB is not IStackable stackB)
                return false;

            if (!itemA.IsSame(itemB))
                return false;

            int maxStackSize = invB[slotB].OverrideStackLimit ? invB[slotB].OverriddenStackLimit : stackB.MaxStackSize;
            if (stackA.StackSize + stackB.StackSize < maxStackSize)
            {
                stackableAmount = stackA.StackSize;
                return true;
            }

            stackableAmount = maxStackSize - stackB.StackSize;
            return true;
        }
    }
}