using Ludole.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ludole.Inventory
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class InventoryGridDisplay : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;

        private List<ItemSlotDisplay> _spawnedObjects;

        public Inventory Inventory
        {
            get => _inventory;
            set
            {
                UnbindEvents();
                _inventory = value;
                BindEvents();
                Rebuild(_inventory);
            }
        }

        [Title("Overrides")] public bool UseCustomSlotPrefab;

        [/*AssetsOnly,*/ ShowIf(nameof(UseCustomSlotPrefab), true)]
        public GameObject CustomSlotPrefab;

        protected virtual void Awake()
        {
            _spawnedObjects = new List<ItemSlotDisplay>();
        }

        protected virtual void Start()
        {
            if (_inventory != null)
            {
                BindEvents();
                Rebuild(_inventory);
            }
        }

        private void BindEvents()
        {
            if (_inventory != null)
                _inventory.OnContentChanged.AddListener(Rebuild);
        }

        private void UnbindEvents()
        {
            if (_inventory != null)
                _inventory.OnContentChanged.RemoveListener(Rebuild);
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

        private void Rebuild(Inventory _)
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

                    _spawnedObjects.Add(refs);
                }

                ItemSlotDisplay itemSlotDisplay = _spawnedObjects[i];
                itemSlotDisplay.Inventory = _inventory;
                itemSlotDisplay.SlotIndex = i;
                itemSlotDisplay.Refresh();
            }
        }
    }
}