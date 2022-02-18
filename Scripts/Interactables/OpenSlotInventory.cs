using Ludole.Core;
using UnityEngine;

namespace Ludole.Inventory
{
    [RequireComponent(typeof(SlotInventory))]
    public class OpenSlotInventory : MonoBehaviour, IInteractable
    {
        public void Interact(IEntity instigator)
        {
            GameObject wnd = Instantiate(Manager.Use<WindowManager>().SlotInventoryPrefab, Manager.Use<WindowManager>().DefaultWindowParent);
            wnd.GetComponentInChildren<InventoryGridDisplay>().Inventory = GetComponent<SlotInventory>();
            Manager.Use<GameStateManager>().ActivateUIMode();
        }
    }
}