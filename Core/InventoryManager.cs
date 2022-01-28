using Ludole.Core;
using UnityEngine;

namespace Ludole.Inventory
{
    public enum TooltipMode { FollowMouse, RelativeToSlot }

    public class InventoryManager : ManagerModule
    {
        [Title("Rarity")] 
        public Rarity DefaultRarity;
        public Rarity[] TrashRarities;

        /*[AssetsOnly]*/ public GameObject GridSlotPrefab;

        [Title("References"), /*SceneObjectsOnly*/] public GameObject Tooltip;
        public Vector2 TooltipOffset = new Vector2(8,8);
        public TooltipMode TooltipMode;
    }
}