using Ludole.Core;
using MarkupAttributes;
using UnityEngine;

namespace Ludole.Inventory
{
    public enum TooltipMode { FollowMouse, RelativeToSlot }

    public class InventoryManager : ManagerModule
    {
        [TitleGroup("Rarities")] 
        public Rarity DefaultRarity;
        public Rarity[] TrashRarities;

        [TitleGroup("Prefabs")]
        public GameObject GridSlotPrefab;

        [TitleGroup("Tooltip")] 
        public GameObject Tooltip;
        public TooltipMode TooltipMode;
        public Vector2 TooltipOffset = new Vector2(8, 8);
    }
}