using Ludole.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ludole.Inventory
{
    public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private IItemSource _itemDisplay;

        protected virtual void Start()
        {
            _itemDisplay = GetComponentInParent<IItemSource>();
            if (_itemDisplay == null)
                throw new MissingComponentException(
                    $"[Inventory] DragHandler on {gameObject.name} has no {nameof(ItemSlotDisplay)} component in any of it's parents");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Place pre-drag checks here
            Manager.Use<DragDropHandler>().RegisterDragOperation(_itemDisplay);
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