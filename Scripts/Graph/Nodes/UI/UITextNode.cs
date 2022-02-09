using GraphProcessor;
using Ludole.Core;
using TMPro;
using UnityEngine;

namespace Ludole.Inventory
{
    [NodeMenuItem("UI/Text")]
    public class UITextNode : FlowNode
    {
        public enum HorizontalAlignmentOptions
        {
            Left = 1,
            Center = 2,
            Right = 4,
            Justified = 8,
            Flush = 16,
            Geometry = 32,
        }

        public enum VerticalAlignmentOptions
        {
            Top = 256,
            Middle = 512,
            Bottom = 1024,
            Baseline = 2048,
            Geometry = 4096,
            Capline = 8192,
        }

        public override string name => "Text Block";

        [Input] public UIGroup Group;
        [Input, SerializeField] public string Text;

        [Input, SerializeField, ColorUsage(true)]
        public Color Color = Color.white;

        [Setting("FontWeight")] public FontWeight FontWeight = FontWeight.Regular;
        [Setting("Font Size")] public float FontSize = 28;
        [Setting("Font Style")] public FontStyles FontStyle = FontStyles.Bold;

        [Setting("Horizontal Alignment")]
        public HorizontalAlignmentOptions HorizontalAlignment = HorizontalAlignmentOptions.Left;

        [Setting("Vertical Alignment")]
        public VerticalAlignmentOptions VerticalAlignment = VerticalAlignmentOptions.Top;

        protected override void Process()
        {
            base.Process();
            RectTransform parent = (RectTransform)(Group != null && Group.Parent != null
                ? Group.Parent
                : ((TooltipGraph)graph).TooltipRoot.transform);
            GameObject obj = UnityEngine.Object.Instantiate(Manager.Use<TooltipManager>().BasicTextPrefab, parent);
            TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
            text.text = Text;
            text.fontWeight = FontWeight;
            text.fontSize = FontSize;
            text.color = Color;
            text.fontStyle = FontStyle;
            text.horizontalAlignment = (TMPro.HorizontalAlignmentOptions)HorizontalAlignment;
            text.verticalAlignment = (TMPro.VerticalAlignmentOptions)VerticalAlignment;
        }
    }
}