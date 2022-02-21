using MarkupAttributes;
using System.Collections.Generic;
using Ludole.Core;
using Ludole.Unity;
using UnityEngine;
using UnityEngine.Events;

namespace Ludole.Inventory
{
    public enum ConstraintMode
    {
        Any,
        All,
        Invert
    }

    public abstract class InventoryBase : MonoBehaviour
    {
        [TitleGroup("Constraints")]
        public List<Category> Constraints;

        public ConstraintMode ConstraintMode;

        [SerializeField, HideInInspector] private string[] _usedEvents;

        [TitleGroup("Events"), EventGroup("Event", nameof(_usedEvents))] public ItemOverflowEvent OnItemOverflow;
        [EventGroup("Event", nameof(_usedEvents))] public InventoryChangedEvent OnContentChanged;
        [EventGroup("Event", nameof(_usedEvents))] public InventorySizeChangedEvent OnSizeChanged;
        [EventGroup("Event", nameof(_usedEvents))] public InventorySelectionEvent OnSelectionChanged;

        [EventGroupButton("Event", nameof(_usedEvents))]
        public int DummyX;

        public abstract bool IsEmpty { get; }
        public abstract bool IsFull { get; }
        public abstract bool AllowSwapping { get; }

        private bool _raiseChangeEvent = true;

        public void BeginUpdate()
        {
            _raiseChangeEvent = false;
        }

        public void EndUpdate()
        {
            _raiseChangeEvent = true;
            Changed();
        }

        public void Changed()
        {
            if (_raiseChangeEvent)
                OnContentChanged.Invoke();
        }

        public abstract void Place(int index, ItemBase item);
        public abstract void Clear(int index);

        public abstract ItemBase this[int index] { get; }
        public abstract int GetStackLimit(IStackable stack, int slot);
    }
}