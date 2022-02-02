using System;
using BlueGraph;
using UnityEngine;

namespace Ludole.Inventory
{
    [Node(Path = "Items", Name = "Rarity Properties")]
    [Tags("Tooltip")]
    public class RarityProperties : Node
    {
        [Input(Editable = false)] public Rarity Rarity;
        [Output] public string Name;
        [Output] public Color Color;
        
        public override object OnRequestValue(Port port)
        {
            switch (port.Name)
            {
                case nameof(Name):
                    return GetInputValue<Rarity>(nameof(Rarity)).Name;
                case nameof(Color):
                    return GetInputValue<Rarity>(nameof(Rarity)).Color;
                default:
                    throw new ArgumentException();
            }
        }
    }
}