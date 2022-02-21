using System;
using Ludole.Core;
using UnityEngine;

namespace Ludole.Inventory
{
    [RequireComponent(typeof(InventoryBase))]
    public class OpenInventory : MonoBehaviour, IInteractable
    {
        public void Interact(IEntity instigator)
        {
            InventoryBase inventory = GetComponent<InventoryBase>();
            GameObject wnd;
            switch (inventory)
            {
                case JigsawInventory jigsawInventory:
                    wnd = Instantiate(Manager.Use<WindowManager>().JigsawInventoryPrefab, Manager.Use<WindowManager>().DefaultWindowParent);
                    wnd.GetComponentInChildren<JigsawGridDisplay>().Inventory = jigsawInventory;
                    break;
                case SlotInventory slotInventory:
                    wnd = Instantiate(Manager.Use<WindowManager>().SlotInventoryPrefab, Manager.Use<WindowManager>().DefaultWindowParent);
                    wnd.GetComponentInChildren<InventoryGridDisplay>().Inventory = slotInventory;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inventory));
            }
          
            Manager.Use<GameStateManager>().ActivateUIMode();
        }
    }
}