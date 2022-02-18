using Ludole.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Ludole.Inventory
{
    [CreateAssetMenu(fileName = "Cooldown Group.asset", menuName = "Inventory/Cooldown Group")]
    public class CooldownGroup : ScriptableObject
    {
        public string Name;

        [Min(0)]public float Cooldown;

        public UnityEvent OnCooldownChanged;
        public bool HasCooldown => Manager.Use<CooldownManager>().RemainingCooldown(this) > 0;
        public float RemainingCooldown => Manager.Use<CooldownManager>().RemainingCooldown(this);
    }
}