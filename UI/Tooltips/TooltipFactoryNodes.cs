using GraphProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ludole.Core;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Ludole.Inventory
{
    public struct TooltipFactoryLink { }

    public interface ITooltipFactoryFlow
    {
        IEnumerable<FlowOutNode> GetExecutedNodes();

        FieldInfo[] GetNodeFields();
    }

    public abstract class FlowOutNode : BaseNode, ITooltipFactoryFlow
    {
        [Output(name = "Flow")]
        public TooltipFactoryLink FlowOut;

        public IEnumerable<FlowOutNode> GetExecutedNodes()
        {
            return outputPorts.FirstOrDefault(n => n.fieldName == nameof(FlowOut))
                ?.GetEdges().Select(e => e.inputNode as FlowOutNode);
        }

        public override FieldInfo[] GetNodeFields()
        {
            FieldInfo[] fields = base.GetNodeFields();
            Array.Sort(fields, (f1, f2) => f1.Name == nameof(FlowOut) ? -1 : 1);
            return fields;
        }
    }

    public abstract class FlowInNode : BaseNode, ITooltipFactoryFlow
    {
        [Output(name = "Flow")]
        public TooltipFactoryLink FlowIn;

        public abstract IEnumerable<FlowOutNode> GetExecutedNodes();

        public override FieldInfo[] GetNodeFields()
        {
            FieldInfo[] fields = base.GetNodeFields();
            Array.Sort(fields, (f1, f2) => f1.Name == nameof(FlowIn) ? -1 : 1);
            return fields;
        }
    }

    public abstract class EventNode : FlowOutNode
    {
    }

    public abstract class FlowNode : FlowOutNode
    {
        [Input(name = "Flow", allowMultiple = true)]
        public TooltipFactoryLink FlowIn;

        public override FieldInfo[] GetNodeFields()
        {
            FieldInfo[] fields = base.GetNodeFields();
            Array.Sort(fields, (f1, f2) => f1.Name == nameof(FlowIn) || f1.Name == nameof(FlowOut) ? -1 : 1);
            return fields;
        }
    }

    [NodeMenuItem("Tooltip Factory/Start")]
    public class FlowOutStartNode : FlowOutNode
    {
        public override string name => "Start";

        [Output(name = "Item")]
        public ItemBase Item;

        protected override void Process()
        {
            base.Process();
            Item = ((TooltipFactoryGraph)graph).Item;
        }
    }

    [Serializable]
    public class UIGroup
    {
        public RectTransform Parent;
    }

    [NodeMenuItem("Tooltip/Content/Text")]
    public class ExecutableAddTextNode : FlowNode
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
        [Input, SerializeField, ColorUsage(true)] public Color Color = Color.white;
        [Setting("FontWeight")] public FontWeight FontWeight = FontWeight.Regular;
        [Setting("Font Size")] public float FontSize = 28;
        [Setting("Font Style")] public FontStyles FontStyle = FontStyles.Bold;

        [Setting("Horizontal Alignment")] public HorizontalAlignmentOptions HorizontalAlignment = HorizontalAlignmentOptions.Left;
        [Setting("Vertical Alignment")] public VerticalAlignmentOptions VerticalAlignment = VerticalAlignmentOptions.Top;

        protected override void Process()
        {
            base.Process();
            RectTransform parent = (RectTransform)(Group != null && Group.Parent != null ? Group.Parent : ((TooltipFactoryGraph)graph).TooltipRoot.transform);
            GameObject obj = UnityEngine.Object.Instantiate(Manager.Use<InventoryManager>().BasicTextPrefab, parent);
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

    [NodeMenuItem("Tooltip/Item/Item Properties")]
    public class TooltipFactorySplitItemNode : BaseNode
    {
        public override string name => "Item Properties";

        [Input] public ItemBase Item;

        [Output] public string Name;
        [Output] public Rarity Rarity;

        protected override void Process()
        {
            base.Process();
            Name = Item.Name;
            Rarity = Item.Rarity;
        }
    }

    [NodeMenuItem("Tooltip/Item/Rarity Properties")]
    public class TooltipFactorySplitRarityNode : BaseNode
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

    [NodeMenuItem("Tooltip/UI/Group")]
    public class CreateGroup : FlowNode
    {
        public enum GroupType { Horizontal, Vertical }

        public override string name => "Create UI Group";

        [Output] public UIGroup Group;
        [SerializeField, Setting("Type")] public GroupType Type;

        protected override void Process()
        {
            base.Process();
            RectTransform parent = (RectTransform)((TooltipFactoryGraph)graph).TooltipRoot.transform;
            Group = new UIGroup();
            switch (Type)
            {
                case GroupType.Horizontal:
                    Group.Parent = UnityEngine.Object.Instantiate(Manager.Use<InventoryManager>().HorizontalGroupPrefab, parent).GetComponent<RectTransform>();
                    break;
                case GroupType.Vertical:
                    Group.Parent = UnityEngine.Object.Instantiate(Manager.Use<InventoryManager>().VerticalGroupPrefab, parent).GetComponent<RectTransform>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}