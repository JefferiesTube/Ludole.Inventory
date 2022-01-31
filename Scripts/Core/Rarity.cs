using UnityEngine;

namespace Ludole.Inventory
{
    [CreateAssetMenu(fileName = "rarity.asset", menuName = "Inventory/Rarity")]
    public class Rarity : ScriptableObject
    {
        public string Name;
        [ColorUsage(false)] public Color Color = Color.white;
        public bool ShowBorderByDefault;
        public int Order;
    }
}