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
                Target.anchoredPosition = _startPos + (CalculateNewPosition() - _startMousePos); ;
                Vector2 sizeDelta = Viewport.sizeDelta - Target.sizeDelta;
                Vector2 position = Target.anchoredPosition;
                position.x = Mathf.Clamp(position.x, -sizeDelta.x * Target.pivot.x, sizeDelta.x * (1 - Target.pivot.x));
                position.y = Mathf.Clamp(position.y, -sizeDelta.y * Target.pivot.y, sizeDelta.y * (1 - Target.pivot.y));
                Target.anchoredPosition = position;
                yield return null;
            }
        }
    }
}
