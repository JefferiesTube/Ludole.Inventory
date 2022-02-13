using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ludole.Inventory
{
    [Serializable]
    public abstract class LootDataset : ScriptableObject
    {
        public float Chance;

        public LootDataset Parent;

        public abstract void AssignLoot(List<LootEntry> loot);
    }
}