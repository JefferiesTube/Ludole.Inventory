using System;
using BlueGraph;
using UnityEditor;

namespace Ludole.Inventory
{
    [Node(Path = "Items", Name = "Item Properties")]
    [Tags("Tooltip")]
    public class ItemProperties : Node
    {
        [Input(Editable = false)] public ItemBase Item;
        [Output] public string Name;
        [Output] public Rarity Rarity;
        
        public override object OnRequestValue(Port port)
        {
            switch (port.Name)
            {
                case nameof(Name):
                    return GetInputValue<ItemBase>(nameof(Item)).Name;
                case nameof(Rarity):
                    return GetInputValue<ItemBase>(nameof(Item)).Rarity;
                default:
                    throw new ArgumentException();
            }
        }
    }
}