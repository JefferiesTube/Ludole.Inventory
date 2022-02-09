using Ludole.Core;
using MarkupAttributes;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ludole.Inventory
{
    public enum TooltipMode
    { FollowMouse, RelativeToSlot }

    [Serializable] public class TooltipFactoryProcessor : UnityEvent<ItemBase, GameObject> { }

    public class InventoryManager : ManagerModule
    {
        [TitleGroup("Rarities")]
        public Rarity DefaultRarity;

        public Rarity[] TrashRarities;

        [TitleGroup("Prefabs")]
        public GameObject GridSlotPrefab;
        public GameObject JigsawSlotPrefab;
        public GameObject JigsawVisualPrefab;

        [TitleGroup("Tooltip")]
        public GameObject Tooltip;

        public TooltipMode TooltipMode;
        public Vector2 TooltipOffset = new Vector2(8, 8);

        public string[] TooltipIgnoreTags;

        //public TooltipFactoryProcessor TooltipFactoryProcessors;
        public TooltipGraph TooltipGraph;

        [SerializeField] public GameObject BasicTextPrefab;
        public GameObject HorizontalGroupPrefab;
        public GameObject VerticalGroupPrefab;
    }
}