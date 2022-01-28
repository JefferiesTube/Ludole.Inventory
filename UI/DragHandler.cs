using Ludole.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ludole.Inventory
{
    public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private ItemSlotDisplay _itemSlotDisplay;

        protected virtual void Start()
        {
            _itemSlotDisplay = GetComponentInParent<ItemSlotDisplay>();
            if (_itemSlotDisplay == null)
                throw new MissingComponentException(
                    $"[Inventory] DragHandler on {gameObject.name} has no {nameof(ItemSlotDisplay)} component in any of it's parents");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Place pre-drag checks here
            Manager.Use<DragDropHandler>().RegisterDragOperation(_itemSlotDisplay);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Manager.Use<DragDropHandler>().UpdateDragOperation();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Manager.Use<DragDropHandler>().RestoreDragOperation();
        }
    }
}