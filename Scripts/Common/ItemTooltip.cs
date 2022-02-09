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
            _tooltip.transform.DestroyAllChildren(Manager.Use<InventoryManager>().TooltipIgnoreTags);
            BuildTooltip();
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

        private void BuildTooltip()
        {
            TooltipGraph graph = Manager.Use<InventoryManager>().TooltipGraph;
            graph.TooltipRoot = _tooltip;
            graph.Item = GetComponentInParent<IItemSource>().GetItem();
            LudoleGraphProcessor processor = new LudoleGraphProcessor(graph);
            processor.Run();
        }

        private void MoveTooltipToSlot()
        {
            RectTransform slotParent = GetComponentInParent<IItemSource>().VisualTransform;
            _tooltip.GetComponent<RectTransform>().position = slotParent.position + new Vector3(slotParent.sizeDelta.x / 2, slotParent.sizeDelta.y / 2, 0)
                + (Vector3)(Manager.Use<InventoryManager>().TooltipOffset);
        }

        private void MoveTooltipToMouse()
        {
            _tooltip.GetComponent<RectTransform>().anchoredPosition = (Vector2)(InputHelper.MousePosition)
                - new Vector2(0, Screen.height) + Manager.Use<InventoryManager>().TooltipOffset;
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