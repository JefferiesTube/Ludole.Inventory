using MarkupAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ludole.Inventory
{
    public class ItemSlotDisplay : MonoBehaviour, IItemSource
    {
        public Image Background;
        public Image Frame;
        public Image RarityFrame;
        public Image Icon;
        public TextMeshProUGUI StackSize;
        public Image Selection;

        [SerializeField] private UnityEvent _onSelect;
        public UnityEvent OnSelect => _onSelect;

        [SerializeField] private UnityEvent _onDeselect;
        public UnityEvent OnDeselect => _onDeselect;
        public GameObject GameObject => gameObject;

        [ReadOnly] public SlotInventory SlotInventory;
        [ReadOnly] public int SlotIndex;

        public ItemSlotDisplayEvent OnDisable;
        public ItemSlotDisplayEvent OnEnable;

        public ItemBase GetItem() => SlotInventory[SlotIndex];

        private ItemBase _oldItem;

        public ItemCooldownChangedEvent OnCooldownChanged;
        public UnityEvent OnCooldownCleared;

        public void Refresh()
        {
            ItemSlot slot = SlotInventory.GetSlot(SlotIndex);
            bool hasItem = slot.Content != null;

            if (_oldItem != null && _oldItem != slot.Content)
            {
                UnbindEvents(_oldItem);
                BindEvents();
            }
            else if (_oldItem == null)
            {
                BindEvents();
            }

            Icon.gameObject.SetActive(hasItem);
            Icon.sprite = hasItem ? slot.Content.Visual : null;

            StackSize.gameObject.SetActive(hasItem && slot.Content is IStackable { StackSize: > 1 });
            StackSize.text = hasItem ? (slot.Content as IStackable)?.StackSize.ToString() : "";

            RarityFrame.gameObject.SetActive(hasItem && slot.Content.Rarity.ShowBorderByDefault);
            RarityFrame.color = hasItem ? slot.Content.Rarity.Color : Color.clear;
        }

        public void Enable()
        {
            OnEnable.Invoke(this);
        }

        public void Disable()
        {
            OnDisable.Invoke(this);
        }

        private void InvokeCooldownChangeEvent()
        {
            OnCooldownChanged.Invoke(((IUsable)GetItem()).CooldownGroup);
        }

        public void BindEvents()
        {
            if (GetItem() is IUsable u && u.CooldownGroup != null)
            {
                u.CooldownGroup.OnCooldownChanged.AddListener(InvokeCooldownChangeEvent);
                _oldItem = GetItem();
            }
        }

        public void UnbindEvents(ItemBase target = null)
        {
            if (target == null)
                target = GetItem();

            if (target is IUsable u && u.CooldownGroup != null)
            {
                OnCooldownCleared.Invoke();
                u.CooldownGroup.OnCooldownChanged.RemoveListener(InvokeCooldownChangeEvent);
            }
        }

        public Transform GetDragDropRootTransform => transform.root.GetComponentInChildren<Canvas>().transform;

        public void ToggleRaycastTarget(bool newState)
        {
            Icon.raycastTarget = newState;
        }

        public GameObject VisualSource => Icon.gameObject;

        public InventoryBase Inventory => SlotInventory;
        public int Index 
        {
            get => SlotIndex;
            set => SlotIndex = value;
        }

        public bool IsFree(int index, ItemBase item) => SlotInventory.GetSlot(index).IsEmpty;

        public bool PassesInventorySpecificCheck(ItemBase item, int index)
        {
            ItemSlot itemSlot = SlotInventory.GetSlot(index);
            if (itemSlot.FixedItemType && !item.IsSame(itemSlot.FixedItemTemplate))
                return false;

            return DropHandler.DoConstraintCheck(item, itemSlot.ConstraintMode, itemSlot.SlotConstraints);
        }

        public RectTransform VisualTransform => GetComponent<RectTransform>();
    }
}