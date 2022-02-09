using Ludole.Core;
using MarkupAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ludole.Inventory
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class JigsawGridDisplay : MonoBehaviour
    {
        [SerializeField] private JigsawInventory _jigsawInventory;

        private List<JigsawSlotDisplay> _spawnedObjects;
        private List<GameObject> _spawnedVisuals;

        [TitleGroup("Overrides")] public bool UseCustomSlotPrefab;

        [AssetsOnly, ShowIf(nameof(UseCustomSlotPrefab), true)]
        public GameObject CustomSlotPrefab;

        private GridLayoutGroup _grid;

        public JigsawInventory JigsawInventory
        {
            get => _jigsawInventory;
            set
            {
                UnbindEvents();
                _jigsawInventory = value;
                BindEvents();
                Rebuild(_jigsawInventory);
            }
        }

        protected virtual void Awake()
        {
            _grid = GetComponent<GridLayoutGroup>();
            _spawnedObjects = new List<JigsawSlotDisplay>();
            _spawnedVisuals = new List<GameObject>();
        }

        protected virtual void Start()
        {
            if (_jigsawInventory != null)
            {
                BindEvents();
                Rebuild(_jigsawInventory);
            }
        }

        private void BindEvents()
        {
            if (_jigsawInventory != null)
                _jigsawInventory.OnContentChanged.AddListener(Rebuild);
        }

        private void UnbindEvents()
        {
            if (_jigsawInventory != null)
                _jigsawInventory.OnContentChanged.RemoveListener(Rebuild);
        }

        public void Clear()
        {
            for (int i = _spawnedObjects.Count - 1; i >= 0; i--)
            {
                if (_spawnedObjects[i] != null)
                    Destroy(_spawnedObjects[i]);
                _spawnedObjects.RemoveAt(i);
            }
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

        private void Rebuild(InventoryBase _)
        {
            if (_jigsawInventory == null)
            {
                Clear();
                return;
            }

            bool doSpawn = _spawnedObjects.Count != _jigsawInventory.Width * _jigsawInventory.Height;
            if (doSpawn)
                Clear();

            for (int i = 0; i < _jigsawInventory.Width * _jigsawInventory.Height; i++)
            {
                Vector2Int position = new Vector2Int(i % _jigsawInventory.Width, i / _jigsawInventory.Width);
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

                    _spawnedObjects.Add(refs);
                }

                JigsawSlotDisplay itemSlotDisplay = _spawnedObjects[i];
                itemSlotDisplay.JigsawInventory = _jigsawInventory;
                itemSlotDisplay.Position = position;
                itemSlotDisplay.Refresh();
            }

            ClearVisuals();
            SpawnVisuals();

            _grid.constraintCount = _jigsawInventory.Width;
        }

        private void SpawnVisuals()
        {
            foreach (JigsawContent item in _jigsawInventory.GetItems())
            {
                GameObject visual = Instantiate(Manager.Use<InventoryManager>().JigsawVisualPrefab);
                RectTransform rt = visual.GetComponent<RectTransform>();
                JigsawSlotDisplay jsd = visual.GetComponent<JigsawSlotDisplay>();
                jsd.JigsawInventory = _jigsawInventory;
                jsd.Position = new Vector2Int(item.X, item.Y);
                rt.SetParent(_grid.transform);

                rt.anchoredPosition = new Vector2(
                       item.X * _grid.cellSize.x + Mathf.Min(0, item.X - 1) * _grid.spacing.x,
                    - (item.Y * _grid.cellSize.y + Mathf.Min(0, item.Y - 1) * _grid.spacing.y));

                rt.sizeDelta = new Vector2(
                    item.Content.Width  * _grid.cellSize.x + Mathf.Min(0, item.Content.Width - 1) * _grid.spacing.x,
                    item.Content.Height * _grid.cellSize.y + Mathf.Min(0, item.Content.Height - 1) * _grid.spacing.y);
                jsd.Image.sprite = item.Content.Visual;
                _spawnedVisuals.Add(visual);
            }
        }
    }
}