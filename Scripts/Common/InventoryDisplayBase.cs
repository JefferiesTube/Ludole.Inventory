using Ludole.Core;
using MarkupAttributes;
using System.Collections.Generic;
using System.Linq;
using Ludole.Unity;
using UnityEngine;

namespace Ludole.Inventory
{
    public abstract class InventoryDisplayBase : MonoBehaviour
    { }

    public abstract class InventoryDisplayBase<TInventory, TDisplay> : InventoryDisplayBase
        where TInventory : InventoryBase
        where TDisplay : IItemSource
    {
        [SerializeField] protected TInventory _inventory;

        protected Dictionary<int, TDisplay> _spawnedObjects;

        private List<int> _selection;

        [TitleGroup("Overrides")] public bool UseCustomSlotPrefab;

        [AssetsOnly, ShowIf(nameof(UseCustomSlotPrefab), true)]
        public GameObject CustomSlotPrefab;

        public bool Initialized { get; private set; }

        protected virtual void Awake()
        {
            _spawnedObjects = new Dictionary<int, TDisplay>();
            _selection = new List<int>();
            Initialized = true;
        }

        public TInventory Inventory
        {
            get => _inventory;
            set
            {
                UnbindEvents();
                _inventory = value;
                BindEvents();
                Rebuild();
            }
        }

        public virtual void BindEvents()
        {
            if (_inventory != null)
            {
                _inventory.OnContentChanged.AddListener(Rebuild);
                _inventory.OnSelectionChanged.AddListener(UpdateSelection);
            }
        }

        public virtual void UnbindEvents()
        {
            if (_inventory != null)
                _inventory.OnContentChanged.RemoveListener(Rebuild);
        }

        public abstract void Rebuild();

        protected virtual void Start()
        {
            if (_inventory != null)
            {
                BindEvents();
                Rebuild();
            }
        }

        public void Clear()
        {
            for (int i = _spawnedObjects.Count - 1; i >= 0; i--)
            {
                if (_spawnedObjects[i] != null)
                    Destroy(_spawnedObjects[i].GameObject);
                _spawnedObjects.Remove(i);
            }
        }

        private void UpdateSelection(IList<int> newSelection)
        {
            List<int> deselectedItems = _selection.Except(newSelection).ToList();
            List<int> newlySelectedItems = newSelection.Except(_selection).ToList();

            foreach (int deselectedIndex in deselectedItems)
            {
                _spawnedObjects[deselectedIndex].OnDeselect.Invoke();
                _selection.Remove(deselectedIndex);
            }

            foreach (int newlySelectedIndex in newlySelectedItems)
            {
                _spawnedObjects[newlySelectedIndex].OnSelect.Invoke();
                _selection.Add(newlySelectedIndex);
            }
        }
    }
}