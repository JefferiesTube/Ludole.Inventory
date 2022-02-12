using System;
using MarkupAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ludole.Inventory
{
    [Serializable] public class VisualEvent : UnityEvent<ItemBase> {}

    public class JigsawSlotDisplay : MonoBehaviour, IItemSource
    {
        public Image Background;
        public Image Image;
        public Image Selection;
        public UIVertexRotation VertexRotator;

        [SerializeField] private UnityEvent _onSelect;
        public UnityEvent OnSelect => _onSelect;
        
        [SerializeField] private UnityEvent _onDeselect;
        public UnityEvent OnDeselect => _onDeselect;
        public GameObject GameObject => gameObject;

        [ReadOnly] public JigsawInventory JigsawInventory;
        [ReadOnly] public Vector2Int Position;

        public Transform GetDragDropRootTransform => transform.root.GetComponentInChildren<Canvas>().transform;

        public RectTransform VisualTransform => GetComponent<RectTransform>();

        public VisualEvent CustomVisualSetup;

        public void Enable()
        {
            if (Image != null)
                Image.enabled = true;
            if (Background != null)
                Background.enabled = true;
        }

        public void Disable()
        {
            if(Image != null)
                Image.enabled = false;
            if (Background != null)
                Background.enabled = false;
        }

        public void ToggleRaycastTarget(bool newState)
        {
            Image.raycastTarget = newState;
        }

        public GameObject VisualSource => Image.gameObject;
        public InventoryBase Inventory => JigsawInventory;

        public int Index
        {
            get => Position.x + Position.y * JigsawInventory.Width;
            set => Position = new Vector2Int(value % JigsawInventory.Width, value / JigsawInventory.Width);
        }

        public bool IsFree(int index, ItemBase item)
        {
            return JigsawInventory.CanPlace(item, index % JigsawInventory.Width, index / JigsawInventory.Width);
        }

        public ItemBase GetItem() => JigsawInventory[Position];

        public bool PassesInventorySpecificCheck(ItemBase item, int index) => true;

        public void Refresh()
        {
        }
    }
}