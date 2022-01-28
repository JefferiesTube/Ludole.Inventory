using Ludole.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ludole.Inventory
{
    public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private GameObject _tooltip;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _tooltip = Manager.Use<InventoryManager>().Tooltip;
            _tooltip.SetActive(true);
            switch (Manager.Use<InventoryManager>().TooltipMode)
            {
                case TooltipMode.FollowMouse:
                    MoveTooltipToMouse();
                    StartCoroutine(MoveTooltip());
                    break;

                case TooltipMode.RelativeToSlot:
                    MoveTooltipToSlot();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void MoveTooltipToSlot()
        {
            RectTransform slotParent = GetComponentInParent<ItemSlotDisplay>().GetComponent<RectTransform>();
            _tooltip.GetComponent<RectTransform>().position = slotParent.position + new Vector3(slotParent.sizeDelta.x / 2, slotParent.sizeDelta.y / 2, 0)
                + (Vector3)(Manager.Use<InventoryManager>().TooltipOffset);
        }

        private void MoveTooltipToMouse()
        {
#if ENABLE_INPUT_SYSTEM
            _tooltip.GetComponent<RectTransform>().anchoredPosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue()
                - new Vector2(0, Screen.height) + Manager.Use<InventoryManager>().TooltipOffset;
#else
            _tooltip.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
#endif
        }

        private IEnumerator MoveTooltip()
        {
            while (true)
            {
                MoveTooltipToMouse();
                yield return null;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Manager.Use<InventoryManager>().Tooltip.SetActive(false);
            StopCoroutine(MoveTooltip());
        }
    }
}