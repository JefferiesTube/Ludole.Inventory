using UnityEngine;

namespace Ludole.Inventory
{
    [CreateAssetMenu(fileName = "category.asset", menuName = "Inventory/Category")]
    public class Category : ScriptableObject
    {
        public string Name;
    }
}