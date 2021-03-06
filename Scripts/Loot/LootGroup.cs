using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Ludole.Inventory
{
    public enum GroupMode { One, Some, All }

    [Serializable]
    public class LootGroup : LootDataset
    {
        public GroupMode Mode;
        [SerializeReference] public List<LootDataset> SubItems;
        public int Limit;
        public string GroupName;
        public override string Name => GroupName;

        public LootGroup() : this("")
        {
        }

        public LootGroup(string name)
        {
            GroupName = name;
            SubItems = new List<LootDataset>();
        }

        public override void AssignLoot(List<LootEntry> loot)
        {
            switch (Mode)
            {
                case GroupMode.One:
                    WeightedPicker<LootDataset> picker = new WeightedPicker<LootDataset>();
                    foreach (LootDataset item in SubItems)
                    {
                        picker.Add(item, item.Chance);
                    }
                    picker.Pick().AssignLoot(loot);
                    break;
                case GroupMode.Some:
                    Random r = new Random(Guid.NewGuid().GetHashCode());
                    int dropCount = 0;

                    List<LootDataset> helper = new List<LootDataset>();
                    helper.AddRange(SubItems);
                    helper.Shuffle();

                    foreach (LootDataset item in helper)
                    {
                        if (r.NextDouble() <= item.Chance)
                        {
                            item.AssignLoot(loot);
                            dropCount++;
                        }
                        if (Limit > 0 && dropCount >= Limit)
                            break;
                    }
                    break;
                case GroupMode.All:
                    foreach (LootDataset item in SubItems)
                    {
                        item.AssignLoot(loot);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}