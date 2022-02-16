using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ludole.Inventory
{
    [Serializable]
    public abstract class LootDataset
    {
        public float Chance;
        public abstract string Name { get; }
        public abstract void AssignLoot(List<LootEntry> loot);
    }
}