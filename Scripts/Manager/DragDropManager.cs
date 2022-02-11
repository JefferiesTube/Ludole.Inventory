using Ludole.Core;
using MarkupAttributes;
using UnityEngine;

namespace Ludole.Inventory
{
    public class DragDropManager : ManagerModule
    {
        private GameObject _dragDropRoot;
        private GameObject _draggedIcon;

        [TitleGroup("Input")]
#if ENABLE_INPUT_SYSTEM
        public UnityEngine.InputSystem.InputActionReference _splitAction;
#else
        public KeyCode[] _splitKeyBindings;
#endif

        [TitleGroup("Settings")] 
        public float DragPreviewIconSize = 200;

        [TitleGroup("Debug")]
        [ReadOnly] public bool IsSplitOperation;
        [ReadOnly] public bool InDragOperation;
        [ReadOnly] public IItemSource DragSource;


        public override void OnAwake()
        {
            base.OnAwake();
            _splitAction.ToInputAction().Enable();
        }

        private void CreateDragIconCopy(IItemSource display)
        {
            _draggedIcon = Instantiate(display.VisualSource, _dragDropRoot.GetComponent<RectTransform>());
            RectTransform rt = _draggedIcon.GetComponent<RectTransform>().SetAnchor(AnchorPresets.TopLeft);
            Vector2 originalSize = display.VisualSource.GetComponent<RectTransform>().rect.size;

            float aspectRatio = originalSize.x / originalSize.y;

            if (originalSize.x > originalSize.y)
            {
                originalSize.y = DragPreviewIconSize;
                originalSize.x = DragPreviewIconSize * aspectRatio;
            }
            else
            {
                originalSize.x = DragPreviewIconSize;
                originalSize.y = DragPreviewIconSize * (1 / aspectRatio);
            }
            rt.sizeDelta = originalSize;
            Destroy(_draggedIcon.GetComponent<DragHandler>());
            display.Disable();
        }

        private void CreateDragDropRoot()
        {
            _dragDropRoot = new GameObject("DragDropRoot", typeof(RectTransform));
            _dragDropRoot.transform.SetParent(DragSource.GetDragDropRootTransform);
            _dragDropRoot.transform.localScale = Vector3.one;
            _dragDropRoot.transform.SetAsLastSibling();
            _dragDropRoot.GetComponent<RectTransform>()
                .SetAnchor(AnchorPresets.StretchAll)
                .SetPivot(PivotPresets.BottomLeft)
                .sizeDelta = Vector2.zero;
        }

        public void RegisterDragOperation(IItemSource display)
        {
            display.ToggleRaycastTarget(false);
            DragSource = display;
            CreateDragDropRoot();
            CreateDragIconCopy(display);
            IsSplitOperation = DetectSplitOperation();
            Manager.Use<WindowManager>().Continuously = true;
            InDragOperation = true;
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
            DragSource.ToggleRaycastTarget(true);
            Destroy(_draggedIcon);
            Destroy(_dragDropRoot);
            DragSource.Enable();
            Manager.Use<WindowManager>().Continuously = false;
            InDragOperation = false;
            // TODO: Events
        }
    }
}