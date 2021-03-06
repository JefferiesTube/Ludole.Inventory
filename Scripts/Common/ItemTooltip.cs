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
            _tooltip = Manager.Use<TooltipManager>().Tooltip;
            _tooltip.transform.DestroyAllChildren(Manager.Use<TooltipManager>().TooltipIgnoreTags);
            BuildTooltip();
            _tooltip.SetActive(true);
            switch (Manager.Use<TooltipManager>().TooltipMode)
            {
                case TooltipMode.FollowMouse:
                    MoveTooltipToMouse();
                    StartCoroutine(MoveTooltip());
                    break;

                case TooltipMode.RelativeToSlot:
                    MoveTooltipToSlot();
                    break;

                case TooltipMode.Fixed:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void BuildTooltip()
        {
            TooltipGraph graph = Manager.Use<TooltipManager>().TooltipGraph;
            graph.TooltipRoot = _tooltip;
            graph.Item = GetComponentInParent<IItemSource>().GetItem();
            LudoleGraphProcessor processor = new LudoleGraphProcessor(graph);
            processor.Run();
            Manager.Use<TooltipManager>().OnOpenTooltip.Invoke(graph.Item);
        }

        private void MoveTooltipToSlot()
        {
            RectTransform slotParent = GetComponentInParent<IItemSource>().VisualTransform;
            _tooltip.GetComponent<RectTransform>().position = slotParent.position + new Vector3(slotParent.sizeDelta.x / 2, slotParent.sizeDelta.y / 2, 0)
                + (Vector3)(Manager.Use<TooltipManager>().TooltipOffset);
        }

        private void MoveTooltipToMouse()
        {
            _tooltip.GetComponent<RectTransform>().anchoredPosition = (Vector2)(InputHelper.MousePosition)
                - new Vector2(0, Screen.height) + Manager.Use<TooltipManager>().TooltipOffset;
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
            Manager.Use<TooltipManager>().Tooltip.SetActive(false);
            StopCoroutine(MoveTooltip());
            Manager.Use<TooltipManager>().OnCloseTooltip.Invoke();

        }
    }
}