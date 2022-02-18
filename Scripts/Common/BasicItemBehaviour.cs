using Ludole.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ludole.Inventory
{
    public class BasicItemBehaviour : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            ItemBase item = GetComponentInParent<ItemSlotDisplay>().GetItem();
            if (item != null && item is IUsable u && eventData.button == PointerEventData.InputButton.Right 
                && (u.CooldownGroup != null &&  u.CooldownGroup.RemainingCooldown <= 0 || u.CooldownGroup == null))
            {
                u.OnUse.Invoke();
                if (u.CooldownGroup != null && u.CooldownGroup.Cooldown > 0)
                {
                    Manager.Use<CooldownManager>().RegisterCooldown(u.CooldownGroup);
                }
            }
        }
    }
}