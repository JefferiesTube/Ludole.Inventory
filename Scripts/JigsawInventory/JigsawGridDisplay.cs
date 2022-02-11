using Ludole.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ludole.Inventory
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class JigsawGridDisplay : InventoryDisplayBase<JigsawInventory, JigsawSlotDisplay>
    {
        private List<GameObject> _spawnedVisuals;

        private GridLayoutGroup _grid;

        protected override void Awake()
        {
            base.Awake();
            _grid = GetComponent<GridLayoutGroup>();
            _spawnedVisuals = new List<GameObject>();
        }

        public void ClearVisuals()
        {
            for (int i = _spawnedVisuals.Count - 1; i >= 0; i--)
            {
                if (_spawnedVisuals[i] != null)
                    Destroy(_spawnedVisuals[i]);
                _spawnedVisuals.RemoveAt(i);
            }
        }

        public override void Rebuild()
        {
            if (_inventory == null)
            {
                Clear();
                return;
            }

            bool doSpawn = _spawnedObjects.Count != _inventory.Width * _inventory.Height;
            if (doSpawn)
                Clear();

            for (int i = 0; i < _inventory.Width * _inventory.Height; i++)
            {
                Vector2Int position = new Vector2Int(i % _inventory.Width, i / _inventory.Width);
                if (doSpawn)
                {
                    GameObject slotPrefab = UseCustomSlotPrefab
                        ? CustomSlotPrefab
                        : Manager.Use<InventoryManager>().JigsawSlotPrefab;
                    GameObject slot = Instantiate(slotPrefab, transform);
                    slot.name = $"{position.x:D2}:{position.y:D2}";
                    JigsawSlotDisplay refs = slot.GetComponent<JigsawSlotDisplay>();
                    //if (refs == null)
                    //    throw new MissingComponentException(
                    //        $"[Inventory] SlotPrefabs require a \'{nameof(ItemSlotDisplay)}\' component");

                    _spawnedObjects.Add(i, refs);
                }

                JigsawSlotDisplay itemSlotDisplay = _spawnedObjects[i];
                itemSlotDisplay.JigsawInventory = _inventory;
                itemSlotDisplay.Position = position;
                itemSlotDisplay.Refresh();
            }

            ClearVisuals();
            SpawnVisuals();

            _grid.constraintCount = _inventory.Width;
        }

        private void SpawnVisuals()
        {
            foreach (JigsawContent item in _inventory.GetItems())
            {
                GameObject visual = Instantiate(Manager.Use<InventoryManager>().JigsawVisualPrefab);
                RectTransform rt = visual.GetComponent<RectTransform>();
                JigsawSlotDisplay jsd = visual.GetComponent<JigsawSlotDisplay>();
                jsd.JigsawInventory = _inventory;
                jsd.Position = new Vector2Int(item.X, item.Y);
                rt.SetParent(_grid.transform);
                //jsd.VertexRotator.transform.localRotation = Quaternion.Euler(0, 0, item.Content.Rotated ? -90 : 0);
                jsd.VertexRotator.Rotation = item.Content.Rotated ? VertexRotation.Quarter : VertexRotation.None;

                rt.anchoredPosition = new Vector2(
                       item.X * _grid.cellSize.x + Mathf.Min(0, item.X - 1) * _grid.spacing.x,
                    -(item.Y * _grid.cellSize.y + Mathf.Min(0, item.Y - 1) * _grid.spacing.y));

                rt.sizeDelta = new Vector2(
                    item.Content.Width * _grid.cellSize.x + Mathf.Min(0, item.Content.Width - 1) * _grid.spacing.x,
                    item.Content.Height * _grid.cellSize.y + Mathf.Min(0, item.Content.Height - 1) * _grid.spacing.y);
                jsd.Image.sprite = item.Content.Visual;
                _spawnedVisuals.Add(visual);
            }
        }

        public int[] GetAffectedSlots(int selectionIndex, ItemBase item)
        {
            return _inventory.GetAffectedSlotIndizes(selectionIndex, item.Width, item.Height);
        }
    }
}