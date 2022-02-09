using Ludole.Core;
using System.Collections.Generic;
using MarkupAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Ludole.Inventory
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class InventoryGridDisplay : MonoBehaviour
    {
        [SerializeField] private SlotInventory _slotInventory;

        private List<ItemSlotDisplay> _spawnedObjects;

        public SlotInventory SlotInventory
        {
            get => _slotInventory;
            set
            {
                UnbindEvents();
                _slotInventory = value;
                BindEvents();
                Rebuild(_slotInventory);
            }
        }

        [TitleGroup("Overrides")] public bool UseCustomSlotPrefab;

        [AssetsOnly, ShowIf(nameof(UseCustomSlotPrefab), true)]
        public GameObject CustomSlotPrefab;

        protected virtual void Awake()
        {
            _spawnedObjects = new List<ItemSlotDisplay>();
        }

        protected virtual void Start()
        {
            if (_slotInventory != null)
            {
                BindEvents();
                Rebuild(_slotInventory);
            }
        }

        private void BindEvents()
        {
            if (_slotInventory != null)
                _slotInventory.OnContentChanged.AddListener(Rebuild);
        }

        private void UnbindEvents()
        {
            if (_slotInventory != null)
                _slotInventory.OnContentChanged.RemoveListener(Rebuild);
        }

        public void Clear()
        {
            for (int i = _spawnedObjects.Count - 1; i >= 0; i--)
            {
                if (_spawnedObjects[i] != null)
                    Destroy(_spawnedObjects[i]);
                _spawnedObjects.RemoveAt(i);
            }
        }

        private void Rebuild(InventoryBase _)
        {
            if (_slotInventory == null)
            {
                Clear();
                return;
            }

            bool doSpawn = _spawnedObjects.Count != _slotInventory.Size;
            if (doSpawn)
                Clear();

            for (int i = 0; i < _slotInventory.Size; i++)
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

                    _spawnedObjects.Add(refs);
                }

                ItemSlotDisplay itemSlotDisplay = _spawnedObjects[i];
                itemSlotDisplay.SlotInventory = _slotInventory;
                itemSlotDisplay.SlotIndex = i;
                itemSlotDisplay.Refresh();
            }
        }
    }
}