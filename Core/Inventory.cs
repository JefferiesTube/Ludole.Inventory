using System;
using System.Collections.Generic;
using System.Linq;
using Ludole.Core;
using UnityEngine;

namespace Ludole.Inventory
{
    public enum ResizeOverflowHandling
    {
        Destroy,
        AllowTemporarySlot,
        InvokeEvent
    }

    public enum ConstraintMode
    {
        Any,
        All,
        Invert
    }

    public enum InventorySlotMode
    {
        Automatic, Manual 
    }

    public class Inventory : MonoBehaviour
    {
        public InventorySlotMode SlotMode;

        [ShowIf(nameof(SlotMode), InventorySlotMode.Manual)] public List<ItemSlot> ManualSlots;

        [SerializeField] private List<ItemSlot> _contents;

        [SerializeField] private int _size;

        public int Size
        {
            get => _size;
            private set => _size = value;
        }

        public ItemSlot this[int slotIndex]
        {
            get => _contents[slotIndex];
            set => _contents[slotIndex] = value;
        }

        public List<Category> Constraints;
        public ConstraintMode ConstraintMode;

        [SerializeField, HideInInspector] private string[] _usedEvents;
        [SerializeField, HideInInspector] private string[] _usedNotifications;

        [EventGroup("Event", nameof(_usedEvents))] public ItemOverflowEvent OnItemOverflow;
        [EventGroup("Event", nameof(_usedEvents))] public InventoryChangedEvent OnContentChanged;
        [EventGroup("Notification", nameof(_usedNotifications))] public InventorySizeChangedEvent OnSizeChanged;

        [EventGroupButton("Event", nameof(_usedEvents))]
        public int DummyX;

        [EventGroupButton("Notification", nameof(_usedNotifications))]
        public int DummyY;

        public bool IsEmpty => _contents.All(s => s.Content == null);
        public bool IsFull => _contents.All(s => s.Content != null);
        public int FreeSlots => _contents.Count(s => s.Content == null);


        protected virtual void Awake()
        {
            _contents = new List<ItemSlot>();
            switch (SlotMode)
            {
                case InventorySlotMode.Automatic:
                    for (int i = 0; i < _size; i++)
                    {
                        _contents.Add(new ItemSlot());
                    }
                    break;
                case InventorySlotMode.Manual:
                    Size = ManualSlots.Count;
                    foreach (ItemSlot slot in ManualSlots)
                    {
                        _contents.Add(slot);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Resize(int newSize, ResizeOverflowHandling overflowHandling)
        {
            void TransferContentFromCutArea(int count, bool raiseEvent)
            {
                for (int i = count; i < _contents.Count; i++)
                {
                    if (GetFirstEmptySlot(out ItemSlot slot))
                    {
                        slot.Content = _contents[i].Content;
                    }
                    else if (raiseEvent)
                    {
                        OnItemOverflow.Invoke(_contents[i]);
                    }
                }
            }

            int TransferContentToNewList(List<ItemSlot> itemSlots)
            {
                int count = Mathf.Min(_contents.Count, newSize);
                for (int i = 0; i < count; i++)
                {
                    itemSlots[i] = _contents[i];
                }

                return count;
            }

            List<ItemSlot> newContent = new List<ItemSlot>(_size);
            switch (overflowHandling)
            {
                case ResizeOverflowHandling.Destroy:
                    int count = TransferContentToNewList(newContent);
                    TransferContentFromCutArea(count, false);
                    break;

                case ResizeOverflowHandling.AllowTemporarySlot:
                    for (int i = 0; i < _contents.Count; i++)
                    {
                        bool isOverflow = i > newSize;
                        newContent[i] = _contents[i];
                        if (isOverflow)
                            newContent[i].IsTemporary = true;
                    }

                    break;

                case ResizeOverflowHandling.InvokeEvent:
                    count = TransferContentToNewList(newContent);
                    TransferContentFromCutArea(count, true);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(overflowHandling), overflowHandling, null);
            }

            if (newSize > _contents.Count)
            {
                for (int i = _contents.Count; i <= newSize; i++)
                {
                    newContent.Add(new ItemSlot());
                }
            }

            _contents = newContent;
            OnSizeChanged.Invoke(this);
            Changed();
        }

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
                OnContentChanged.Invoke(this);
        }

        public bool GetFirstEmptySlot(out ItemSlot slot)
        {
            for (int i = 0; i < _contents.Count; i++)
            {
                if (_contents[i].Content == null)
                {
                    slot = _contents[i];
                    return true;
                }
            }

            slot = null;
            return false;
        }

        public bool Put(ItemBase item, bool allowPartial)
        {
            // TODO: Try to fill other stacks first
            if (GetFirstEmptySlot(out ItemSlot slot))
            {
                // TODO: Check if stack size is possible to put into this slot
                slot.Content = item;
                Changed();
                return true;
            }

            return false;
        }

        public void ClearSlot(int slotIndex)
        {
            this[slotIndex].Content = null;
        }
    }
}