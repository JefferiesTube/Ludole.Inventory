using Ludole.Core;
using UnityEngine;

namespace Ludole.Inventory
{
    [RequireComponent(typeof(JigsawInventory))]
    public class JigsawContainer : MonoBehaviour, IInteractable
    {
        private JigsawInventory _inventory;
        public JigsawInventory Inventory => _inventory ??= GetComponent<JigsawInventory>();

        public void Interact(IEntity instigator)
        {
            Manager.Use<InventoryManager>().SetCurrentInventory(Inventory);
            Manager.Use<GameStateManager>().ActivateUIMode();
            Manager.Use<GameStateManager>().OnDeactivateUIMode.AddListener(CloseWindow);
        }

        private void CloseWindow()
        {
            Manager.Use<GameStateManager>().OnDeactivateUIMode.RemoveListener(CloseWindow);
            Manager.Use<InventoryManager>().SetCurrentInventory(null);
        }
    }
}