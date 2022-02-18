using UnityEngine;
using UnityEngine.Events;

namespace Ludole.Inventory
{
    [CreateAssetMenu(fileName = "Cooldown Group.asset", menuName = "Inventory/Cooldown Group")]
    public class CooldownGroup : ScriptableObject
    {
        public string Name;

        [Min(0)]public float Cooldown;

        public float RemainingCooldown { get; set; }

        public UnityEvent OnCooldownChanged;
    }
}