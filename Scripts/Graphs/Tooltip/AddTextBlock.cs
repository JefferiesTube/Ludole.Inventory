using BlueGraph;
using Ludole.Core;
using TMPro;
using UnityEngine;

namespace Ludole.Inventory
{
    [Node(Path = "Tooltip")]
    [Tags("Tooltip")]
    public class AddTextBlock : ExecutableNode
    {
        [Input] public string Text;
        [Input] public Color Color = Color.white;
        [Input] public FontWeight FontWeight = FontWeight.Regular;
        [Input] public float FontSize = 28;
        [Input] public FontStyles FontStyle = FontStyles.Bold;

        public override IExecutableNode Execute(ExecutionFlowData data)
        {
            GameObject obj = Object.Instantiate(Manager.Use<InventoryManager>().BasicTextPrefab, Manager.Use<InventoryManager>().TooltipRoot.transform);
            TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
            text.text = GetInputValue(nameof(Text), "");
            text.color = GetInputValue(nameof(Color), Color.white); ;
            text.fontWeight = GetInputValue(nameof(FontWeight), FontWeight.Regular); ;
            text.fontSize = GetInputValue(nameof(FontSize), 28); ;
            text.fontStyle = GetInputValue(nameof(FontStyle), FontStyles.Bold); ;
            return base.Execute(data);
        }
    }
}