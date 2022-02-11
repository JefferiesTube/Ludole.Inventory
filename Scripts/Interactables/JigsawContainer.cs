using Ludole.Core;
using UnityEngine;

namespace Ludole.Inventory
{
    [RequireComponent(typeof(JigsawInventory))]
    public class JigsawContainer : MonoBehaviour, IInteractable
    {
        private JigsawInventory _inventory;
        public JigsawInventory Inventory => _inventory ??= GetComponent<JigsawInventory>();

        private GameObject _uiWindow;

        public void Interact(IEntity instigator)
        {
            if (_uiWindow == null)
            {
                _uiWindow = Instantiate(Manager.Use<WindowManager>().JigsawInventoryPrefab,
                    Manager.Use<WindowManager>().DefaultWindowParent);
                JigsawGridDisplay display = _uiWindow.GetComponentInChildren<JigsawGridDisplay>();
                display.Inventory = Inventory;
                display.Rebuild();
            }

            _uiWindow.SetActive(true);
            Manager.Use<GameStateManager>().ActivateUIMode();
            Manager.Use<GameStateManager>().OnDeactivateUIMode.AddListener(CloseWindow);
        }

        private void CloseWindow()
        {
            Manager.Use<GameStateManager>().OnDeactivateUIMode.RemoveListener(CloseWindow);
            Destroy(_uiWindow);
        }
    }
}