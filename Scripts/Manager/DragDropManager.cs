using Ludole.Core;
using MarkupAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ludole.Inventory
{
    public class DragDropManager : ManagerModule
    {
        private GameObject _dragDropRoot;
        private GameObject _draggedIcon;

        [TitleGroup("Input")]
        public InputActionReference _splitAction;
        public InputActionReference _rotateAction;

        [TitleGroup("Settings")]
        public float DragPreviewIconSize = 200;

        [TitleGroup("Debug")]
        [ReadOnly] public bool IsSplitOperation;

        [ReadOnly] public bool InDragOperation;
        [ReadOnly] public IItemSource DragSource;

        private bool _wasRotatedOriginally;

        public override void OnAwake()
        {
            base.OnAwake();
            _splitAction.ToInputAction().Enable();
            _rotateAction.ToInputAction().Disable();
            _rotateAction.ToInputAction().performed += RotateDraggedItem;
        }

        private void RotateDraggedItem(InputAction.CallbackContext obj)
        {
            if (!InDragOperation)
                return;

            ItemBase item = DragSource.GetItem();
            if (item.CanRotate)
                item.Rotated = !item.Rotated;
        }

        private void CreateDragIconCopy(IItemSource display)
        {
            _draggedIcon = Instantiate(display.VisualSource, _dragDropRoot.GetComponent<RectTransform>());
            _draggedIcon.name = "Dragged Visual Copy";
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
            if (InDragOperation)
                RestoreDragOperation(false);

            display.ToggleRaycastTarget(false);
            DragSource = display;
            CreateDragDropRoot();
            CreateDragIconCopy(display);
            IsSplitOperation = DetectSplitOperation();
            Manager.Use<WindowManager>().Continuously = true;
            InDragOperation = true;
            _wasRotatedOriginally = display.GetItem().Rotated;
            _rotateAction.ToInputAction().Enable();
            // TODO: Events
        }

        public void UpdateDragOperation()
        {
            if (_draggedIcon != null)
                _draggedIcon.transform.position = InputHelper.MousePosition;
        }

        private bool DetectSplitOperation()
        {
            InputAction inputAction = _splitAction.ToInputAction();
            return inputAction.IsPressed();
        }

        public void RestoreDragOperation(bool operationSuccessful)
        {
            if (DragSource != null)
            {
                DragSource.ToggleRaycastTarget(true);
                if (!operationSuccessful && DragSource.GetItem() != null)
                {
                    DragSource.GetItem().Rotated = _wasRotatedOriginally;
                }

                DragSource.Enable();
            }

            Destroy(_draggedIcon);
            Destroy(_dragDropRoot);
            
            Manager.Use<WindowManager>().Continuously = false;
            InDragOperation = false;
            _rotateAction.ToInputAction().Disable();
            // TODO: Events
        }
    }
}