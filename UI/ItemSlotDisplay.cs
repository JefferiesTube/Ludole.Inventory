using MarkupAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ludole.Inventory
{
    public class ItemSlotDisplay : MonoBehaviour
    {
        public Image Background;
        public Image Frame;
        public Image RarityFrame;
        public Image Icon;
        public TextMeshProUGUI StackSize;

        [ReadOnly] public Inventory Inventory;
        [ReadOnly] public int SlotIndex;

        public ItemSlotDisplayEvent OnDisable;
        public ItemSlotDisplayEvent OnEnable;

        public ItemBase GetItem() => Inventory[SlotIndex].Content;

        public void Refresh()
        {
            ItemSlot slot = Inventory[SlotIndex];
            bool hasItem = slot.Content != null;

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
    }
}