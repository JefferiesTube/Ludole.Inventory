using System.Collections.Generic;
using MarkupAttributes;
using UnityEngine;

namespace Ludole.Inventory
{
    [CreateAssetMenu(fileName = "item.asset", menuName = "Inventory/Item")]
    public class ItemBase : ScriptableObject
    {
        public string Name;
        public long Value;
        public List<Category> Categories;
        public Sprite Visual;
        public GameObject Prefab;
        public Rarity Rarity;
        [Multiline] public string Description;

        [Min(1), SerializeField] private int _width = 1;
        [Min(1), SerializeField] private int _height = 1;

        public int Width => Rotated ? _height : _width;
        public int Height => Rotated ? _width : _height;

        public bool CanRotate;
        [EnableIf(nameof(CanRotate)), SerializeField] private bool _rotated;

        public bool Rotated
        {
            get => _rotated;
            set => _rotated = CanRotate ? value : _rotated;
        }

        public virtual bool IsSame(ItemBase other)
        {
            return Name.Equals(other.Name);
        }
    }
}