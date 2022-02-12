using Ludole.Core;
using MarkupAttributes;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ludole.Inventory
{
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
        public JigsawGridDisplay CurrentInventoryDisplay;

        public void SetCurrentInventory(JigsawInventory inventory)
        {
            if (CurrentInventoryDisplay.Initialized)
            {
                CurrentInventoryDisplay.Inventory = inventory;
            }
            else
            {
                Manager.Use<Executor>().ExecOnEndOfFrame(() => CurrentInventoryDisplay.Inventory = inventory);
            }
        }
    }
}