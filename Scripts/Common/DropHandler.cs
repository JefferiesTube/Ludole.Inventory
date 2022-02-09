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
        private IItemSource Target;
        private IItemSource Source;

        private bool _sourceChanged;
        private bool _targetChanged;
        [HideInInspector] public bool _isSplitOperation;
        

        protected virtual void Start()
        {
            Target = GetComponentInParent<IItemSource>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            try
            {
                _sourceChanged = false;
                _targetChanged = false;

                Source = Manager.Use<DragDropManager>().DragSource;
                _isSplitOperation = Manager.Use<DragDropManager>().IsSplitOperation;

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
                Manager.Use<DragDropManager>().RestoreDragOperation();
            }
        }

        private bool OperateOnSameSlot() => !UsesDifferentInventories() && Source.Index == Target.Index;

        private bool UsesDifferentInventories() => Source.Inventory != Target.Inventory;

        private bool TargetSlotIsFree() => Target.IsFree(Target.Index, Source.GetItem());

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
            Target.Inventory.Place(Target.Index, Instantiate(Source.GetItem()));
            // TODO: Stacking
            // TODO: TrashSlot
            _targetChanged = true;
        }

        private void RemoveSourceItem()
        {
            Source.Inventory.Clear(Source.Index);
            _sourceChanged = true;
        }

        private void SwapItems()
        {
            if (CanStack(Source.Inventory, Source.Index, Target.Inventory, Target.Index, out int amount))
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
                Target.Inventory.Place(Target.Index, Source.GetItem());
                Source.Inventory.Place(Source.Index, tmpItem);
                _targetChanged = true;
                _sourceChanged = true;
            }
        }

        public static bool DoConstraintCheck(ItemBase item, ConstraintMode mode, List<Category> constraints)
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

        private bool PassesConstraintCheck(ItemBase item, InventoryBase inventory)
        {
            return DoConstraintCheck(item, inventory.ConstraintMode, inventory.Constraints);
        }

        private bool TargetInventorySuitable() => PassesConstraintCheck(Source.GetItem(), Target.Inventory)
                                                  && Target.PassesInventorySpecificCheck(Source.GetItem(), Target.Index);

        private bool SwapSuitable() =>
            PassesConstraintCheck(Source.GetItem(), Target.Inventory)
            && PassesConstraintCheck(Target.GetItem(), Source.Inventory)
            && Target.PassesInventorySpecificCheck(Source.GetItem(), Target.Index)
            && Source.PassesInventorySpecificCheck(Target.GetItem(), Source.Index);

        private bool CanStack(InventoryBase invA, int slotA, InventoryBase invB, int slotB, out int stackableAmount)
        {
            stackableAmount = 0;
            ItemBase itemA = invA[slotA];
            ItemBase itemB = invB[slotB];

            if (itemA is not IStackable stackA)
                return false;
            if (itemB is not IStackable stackB)
                return false;

            if (!itemA.IsSame(itemB))
                return false;

            int maxStackSize = invB.GetStackLimit(stackB, slotB); // invB[slotB].OverrideStackLimit ? invB[slotB].OverriddenStackLimit : stackB.MaxStackSize;

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