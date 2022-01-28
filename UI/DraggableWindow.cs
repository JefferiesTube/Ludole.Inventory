using System.Collections;
using Ludole.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ludole.Inventory
{
    public class DraggableWindow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public RectTransform Target;
        private RectTransform Viewport;
        private Vector2 _startMousePos;
        private Vector2 _startPos;


        public static bool IsDragging = false;

        void Start()
        {
            Viewport = gameObject.transform.root.GetComponentInChildren<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsDragging)
            {
                IsDragging = true;
                // TODO: Focus window!
                _startPos = Target.anchoredPosition;
                _startMousePos = CalculateNewPosition();
                Debug.Log($"StartPos = {_startPos}");
                Debug.Log($"StartMousePos = {_startMousePos}");
                StartCoroutine(MoveWindow());
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsDragging = false;
            StopCoroutine(MoveWindow());
            Debug.Log("MAKE IT STOOOOP");
        }

        private Vector2 CalculateNewPosition()
        {
            Canvas parentCanvas = GetComponentInParent<CanvasScaler>().GetComponent<Canvas>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform,
                InputHelper.MousePosition, null, out Vector2 pos);
            return pos;
        }

        private IEnumerator MoveWindow()
        {
            while (IsDragging)
            {
                Vector2 pos = CalculateNewPosition();
                pos = pos - _startMousePos;
                pos = _startPos + pos;
                Target.anchoredPosition = new Vector2(
                Mathf.Clamp(pos.x, -Target.sizeDelta.x, Viewport.sizeDelta.x),
                Mathf.Clamp(pos.y, -Viewport.sizeDelta.y, 0));
                yield return null;
            }
        }
    }
}
