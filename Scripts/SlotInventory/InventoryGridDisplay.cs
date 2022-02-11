using Ludole.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Ludole.Inventory
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class InventoryGridDisplay : InventoryDisplayBase<SlotInventory, ItemSlotDisplay>
    {
        public override void Rebuild()
        {
            if (_inventory == null)
            {
                Clear();
                return;
            }

            bool doSpawn = _spawnedObjects.Count != _inventory.Size;
            if (doSpawn)
                Clear();

            for (int i = 0; i < _inventory.Size; i++)
            {
                if (doSpawn)
                {
                    GameObject slotPrefab = UseCustomSlotPrefab
                        ? CustomSlotPrefab
                        : Manager.Use<InventoryManager>().GridSlotPrefab;
                    GameObject slot = Instantiate(slotPrefab, transform);
                    slot.name = $"{i:D2}_Slot";
                    ItemSlotDisplay refs = slot.GetComponent<ItemSlotDisplay>();
                    if (refs == null)
                        throw new MissingComponentException(
                            $"[Inventory] SlotPrefabs require a \'{nameof(ItemSlotDisplay)}\' component");

                    _spawnedObjects.Add(i, refs);
                }

                ItemSlotDisplay itemSlotDisplay = _spawnedObjects[i];
                itemSlotDisplay.SlotInventory = _inventory;
                itemSlotDisplay.SlotIndex = i;
                itemSlotDisplay.Refresh();
            }
        }
    }
}