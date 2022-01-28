using Ludole.Core;
using MarkupAttributes;
using UnityEngine;

namespace Ludole.Inventory
{
    public class DragDropHandler : ManagerModule
    {
        private GameObject _dragDropRoot;
        private GameObject _draggedIcon;

        [TitleGroup("Input")]
#if ENABLE_INPUT_SYSTEM
        public UnityEngine.InputSystem.InputActionReference _splitAction;
#else
        public KeyCode[] _splitKeyBindings;
#endif

        [TitleGroup("Debug")]
        [ReadOnly] public ItemSlotDisplay DragSource;
        [ReadOnly] public bool IsSplitOperation;

        public override void OnAwake()
        {
            base.OnAwake();
            _splitAction.ToInputAction().Enable();
        }

        private void CreateDragIconCopy(ItemSlotDisplay display)
        {
            _draggedIcon = Instantiate(display.Icon.gameObject, _dragDropRoot.GetComponent<RectTransform>());
            _draggedIcon.GetComponent<RectTransform>().SetAnchor(AnchorPresets.TopLeft).sizeDelta =
                display.Icon.GetComponent<RectTransform>().rect.size;
            Destroy(_draggedIcon.GetComponent<DragHandler>());
            display.Disable();
        }

        private void CreateDragDropRoot()
        {
            _dragDropRoot = new GameObject("DragDropRoot", typeof(RectTransform));
            _dragDropRoot.transform.SetParent(DragSource.transform.root.GetComponentInChildren<Canvas>().transform);
            _dragDropRoot.transform.localScale = Vector3.one;
            _dragDropRoot.transform.SetAsLastSibling();
            _dragDropRoot.GetComponent<RectTransform>()
                .SetAnchor(AnchorPresets.StretchAll)
                .SetPivot(PivotPresets.BottomLeft)
                .sizeDelta = Vector2.zero;
        }

        public void RegisterDragOperation(ItemSlotDisplay display)
        {
            display.Icon.raycastTarget = false;
            DragSource = display;
            CreateDragDropRoot();
            CreateDragIconCopy(display);
            IsSplitOperation = DetectSplitOperation();
            // TODO: Events
        }

        public void UpdateDragOperation()
        {
            _draggedIcon.transform.position = InputHelper.MousePosition;
        }

        private bool DetectSplitOperation()
        {
#if ENABLE_INPUT_SYSTEM
            UnityEngine.InputSystem.InputAction inputAction = _splitAction.ToInputAction();
            return inputAction.IsPressed();
#else
            return _splitKeyBindings.Any(b => Input.GetKey(b));
#endif
        }

        public void RestoreDragOperation()
        {
            DragSource.Icon.raycastTarget = true;
            Destroy(_draggedIcon);
            Destroy(_dragDropRoot);
            DragSource.Enable();
            // TODO: Events
        }
    }
}