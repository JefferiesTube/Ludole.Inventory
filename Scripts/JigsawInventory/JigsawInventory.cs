using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MarkupAttributes;
using UnityEngine;

namespace Ludole.Inventory
{
    public static class JigsawUtility
    {
        public static IEnumerable<Vector2Int> OccupiedSlots(int originX, int originY, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    yield return new Vector2Int(originX + x, originY + y);
                }
            }
        }

        public static Vector2Int IndexToPosition(int index, JigsawInventory inventory) => new Vector2Int(index % inventory.Width, index / inventory.Width);
        public static int PositionToIndex(Vector2Int position, JigsawInventory inventory) => PositionToIndex(position.x, position.y, inventory);
        public static int PositionToIndex(int x, int y, JigsawInventory inventory) => x + y * inventory.Width;
    }

    [Serializable]
    public class JigsawContent
    {
        public int X;
        public int Y;
        [ReadOnly] public ItemBase Content;
    }

    public class JigsawInventory : InventoryBase
    {
        [Min(1)] public int Width = 1;
        [Min(1)] public int Height = 1;

        [TitleGroup("Debug"), SerializeField, ReadOnly]
        private List<JigsawContent> _contents;

        public override bool IsEmpty { get; }
        public override bool IsFull { get; }

        public override bool AllowSwapping => false;

        public IEnumerable<JigsawContent> GetItems()
        {
            foreach (JigsawContent jigsawContent in _contents)
            {
                yield return jigsawContent;
            }
        }

        protected virtual void Awake()
        {
            _contents = new List<JigsawContent>();
        }

        public ItemBase this[int x, int y] =>
            _contents.FirstOrDefault(c =>
                JigsawUtility.OccupiedSlots(c.X, c.Y, c.Content.Width, c.Content.Height)
                    .Contains(new Vector2Int(x, y)))?.Content;

        public ItemBase this[Vector2Int position] => this[position.x, position.y];

        public override void Place(int index, ItemBase item)
        {
            Place(JigsawUtility.IndexToPosition(index, this), item);
        }

        public override void Clear(int index)
        {
            if (FindItem(index, out ItemBase item))
            {
                _contents.RemoveAll(r => r.Content == item);
            }
        }

        public override int GetStackLimit(IStackable stack, int slot)
        {
            return stack.MaxStackSize;
        }

        public override ItemBase this[int index] => this[index % Width, index / Width];

        public bool FindItem(int index, out ItemBase item)
        {
            Vector2Int pos = JigsawUtility.IndexToPosition(index, this);
            item = _contents
                .Select(c => new { Item = c.Content, Slots = JigsawUtility.OccupiedSlots(c.X, c.Y, c.Content.Width, c.Content.Height) })
                .FirstOrDefault(c => c.Slots.Contains(pos))
                ?.Item;
            return item != null;
        }

        public bool IsFree(int x, int y)
        {
            Vector2Int pos = new Vector2Int(x, y);
            return !_contents
                .SelectMany(c => JigsawUtility.OccupiedSlots(c.X, c.Y, c.Content.Width, c.Content.Height))
                .Any(c => c.Equals(pos));
        }

        public bool CanPlace(ItemBase item, int x, int y)
        {
            return !GetAffectedSlotIndizes(JigsawUtility.PositionToIndex(x,y, this), item.Width, item.Height)
                .Select(s => JigsawUtility.IndexToPosition(s, this))
                .Intersect(_contents.Where(c => c.Content != item).SelectMany(c => JigsawUtility.OccupiedSlots(c.X, c.Y, c.Content.Width, c.Content.Height)))
                .Any();
        }

        public Vector2Int GetAdjustedPosition(int desiredIndex, ItemBase item)
        {
            return JigsawUtility.IndexToPosition(GetAffectedSlotIndizes(desiredIndex, item.Width, item.Height).Min(), this);
        }

        public bool Put(ItemBase item)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (CanPlace(item, x, y))
                    {
                        Vector2Int actualPosition = GetAdjustedPosition(JigsawUtility.PositionToIndex(x, y, this), item);
                        JigsawContent c = new JigsawContent
                        {
                            Content = item,
                            X = actualPosition.x,
                            Y = actualPosition.y
                        };

                        _contents.Add(c);
                        Changed();
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Place(Vector2Int position, ItemBase item)
        {
            position = GetAdjustedPosition(JigsawUtility.PositionToIndex(position, this), item);

            JigsawContent c = new JigsawContent
            {
                Content = item,
                X = position.x,
                Y = position.y
            };

            _contents.Add(c);
            Changed();

            return true;
        }

        public int[] GetAffectedSlotIndizes(int desiredStartIndex, int itemWidth, int itemHeight)
        {
            Vector2Int indexedPosition = new Vector2Int(desiredStartIndex % Width, desiredStartIndex / Width);
            int availableWidth = Width - indexedPosition.x;
            if (availableWidth < itemWidth)
            {
                indexedPosition.x -= (itemWidth - availableWidth);
            }

            int availableHeight = Height - indexedPosition.y;
            if (availableHeight < itemHeight)
            {
                indexedPosition.y -= (itemHeight - availableHeight);
            }

            List<int> indizes = new List<int>();

            for (int x = 0; x < itemWidth; x++)
            {
                for (int y = 0; y < itemHeight; y++)
                {
                    Vector2Int position = indexedPosition + new Vector2Int(x,y);
                    indizes.Add(position.x + position.y * Width);
                }
            }

            return indizes.ToArray();
        }
    }
}
