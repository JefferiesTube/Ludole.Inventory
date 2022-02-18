using System.Collections;
using System.Collections.Generic;
using Ludole.Core;
using UnityEngine;

namespace Ludole.Inventory
{
    public class CooldownManager : ManagerModule
    {
        private readonly List<CooldownGroup> _cooldownGroups = new List<CooldownGroup>();

        public void RegisterCooldown(CooldownGroup group)
        {
            if(!_cooldownGroups.Contains(group))
                _cooldownGroups.Add(group);

            group.RemainingCooldown = group.Cooldown;
        }

        void Update()
        {
            ProcessCooldowns();
        }

        private void ProcessCooldowns()
        {
            for (int i = _cooldownGroups.Count - 1; i >= 0; i--)
            {
                _cooldownGroups[i].RemainingCooldown -= Time.deltaTime;
                _cooldownGroups[i].OnCooldownChanged.Invoke();

                if (_cooldownGroups[i].RemainingCooldown <= 0)
                {
                    _cooldownGroups.RemoveAt(i);
                }
            }
        }
    }
}
