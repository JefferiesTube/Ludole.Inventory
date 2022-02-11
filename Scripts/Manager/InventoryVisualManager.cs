using System;
using Ludole.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ludole.Inventory
{
    public class InventoryVisualManager : ManagerModule
    {
        [SerializeField] private GraphicRaycaster _graphicRaycaster;
        [SerializeField] private EventSystem _eventSystem;

        private List<RaycastResult> _raycastResults;

        private List<IItemSource> _selection;
        private InventoryBase _selectedInventory;
        private DragDropManager _dragDropManager;

        public override void OnAwake()
        {
            base.OnAwake();
            _raycastResults = new List<RaycastResult>();
        }

        void Start()
        {
            _dragDropManager = Manager.Use<DragDropManager>();
            _selection = new List<IItemSource>();
        }

        protected virtual void Update()
        {
            FocusSlotBelowCursor();
        }

        private void FocusSlotBelowCursor()
        {
            void ClearSelection()
            {
                _selection.ForEach(i => i.OnDeselect.Invoke());
                _selection.Clear();
            }

            _raycastResults.Clear();
            _graphicRaycaster.Raycast(new PointerEventData(_eventSystem) { position = InputHelper.MousePosition }, _raycastResults);
            var relevantHit = _raycastResults
                .Where(r => r.isValid)
                .Select(r => new { ItemSource = r.gameObject.GetComponent<IItemSource>(), WindowBase = r.gameObject.GetComponent<WindowBase>() })
                .FirstOrDefault(w => w.ItemSource != null || w.WindowBase);
            if (relevantHit?.ItemSource != null)
            {
                ChangeSelection(relevantHit.ItemSource);
            }

            if (relevantHit == null || relevantHit.WindowBase)
            {
                ClearSelection();
            }
        }

        private void ChangeSelection(IItemSource itemSource)
        {
            if (itemSource.Inventory != _selectedInventory)
            {
                if(_selectedInventory)
                    _selectedInventory.OnSelectionChanged.Invoke(new List<int>());
                _selectedInventory = itemSource.Inventory;
            }

            List<int> newSelection = new List<int>();

            switch (itemSource.Inventory)
            {
                case JigsawInventory jigsawInventory:
                    ItemBase item;
                    if (_dragDropManager.InDragOperation)
                    {
                        item = _dragDropManager.DragSource.GetItem();
                        newSelection = jigsawInventory.GetAffectedSlotIndizes(itemSource.Index, item.Width, item.Height).ToList();
                    }
                    else
                    {
                        item = itemSource.GetItem();
                        newSelection = item
                            ? jigsawInventory.GetAffectedSlotIndizes(itemSource.Index, item.Width, item.Height).ToList() 
                            : new List<int> { itemSource.Index };
                    }
                    
                    break;
                case SlotInventory slotInventory:
                    newSelection.Add(itemSource.Index);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            itemSource.Inventory.OnSelectionChanged.Invoke(newSelection);
        }
    }
}