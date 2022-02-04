using GraphProcessor;
using Ludole.Core;

namespace Ludole.Inventory
{
    [NodeMenuItem("Items/Item Properties")]
    public class SplitItemNode : DataNode
    {
        public override string name => "Item Properties";

        [Input] public ItemBase Item;

        [Output] public string Name;
        [Output] public Rarity Rarity;
        [Output] public string Description;

        protected override void Process()
        {
            base.Process();
            Name = Item.Name;
            Rarity = Item.Rarity;
            Description = Item.Description;
        }
    }
}