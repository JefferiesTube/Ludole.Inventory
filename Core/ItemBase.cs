using System.Collections.Generic;
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

        public virtual bool IsSame(ItemBase other)
        {
            return Name.Equals(other.Name);
        }
    }
}