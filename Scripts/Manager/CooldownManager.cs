using System.Collections;
using System.Collections.Generic;
using Ludole.Core;
using UnityEngine;

namespace Ludole.Inventory
{
    public class CooldownManager : ManagerModule
    {
        private readonly List<CooldownGroup> _cooldownGroups = new List<CooldownGroup>();

        private readonly Dictionary<CooldownGroup, float> _remainingCooldowns = new Dictionary<CooldownGroup, float>();

        public void RegisterCooldown(CooldownGroup group)
        {
            if(!_cooldownGroups.Contains(group))
                _cooldownGroups.Add(group);

            if(!_remainingCooldowns.ContainsKey(group))
                _remainingCooldowns.Add(group, group.Cooldown);

            _remainingCooldowns[group] = group.Cooldown;
        }

        void Update()
        {
            ProcessCooldowns();
        }

        private void ProcessCooldowns()
        {
            for (int i = _cooldownGroups.Count - 1; i >= 0; i--)
            {
                _remainingCooldowns[_cooldownGroups[i]] -= Time.deltaTime;
                _cooldownGroups[i].OnCooldownChanged.Invoke();

                if (_remainingCooldowns[_cooldownGroups[i]] <= 0)
                {
                    _cooldownGroups.RemoveAt(i);
                }
            }
        }

        public float RemainingCooldown(CooldownGroup group)
        {
            if(_remainingCooldowns.ContainsKey(group))
                return _remainingCooldowns[group];

            return 0;
        }
    }
}
