using GraphProcessor;
using UnityEngine;

namespace Ludole.Inventory
{
    [NodeMenuItem("Items/Rarity Properties")]
    public class SplitRarityNode : DataNode
    {
        public override string name => "Rarity Properties";

        [Input] public Rarity Rarity;

        [Output] public string Name;
        [Output] public Color Color;

        protected override void Process()
        {
            base.Process();
            Name = Rarity.Name;
            Color = Rarity.Color;
        }
    }
}