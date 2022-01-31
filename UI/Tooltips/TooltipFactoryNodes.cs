using GraphProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ludole.Core;
using TMPro;
using UnityEngine;

namespace Ludole.Inventory
{
    public struct TooltipFactoryLink { }

    public interface ITooltipFactoryFlow
    {
        IEnumerable<TooltipFactoryNode> GetExecutedNodes();

        FieldInfo[] GetNodeFields();
    }

    public abstract class TooltipFactoryNode : BaseNode, ITooltipFactoryFlow
    {
        [Output(name = "Flow")]
        public TooltipFactoryLink FlowOut;

        public abstract IEnumerable<TooltipFactoryNode> GetExecutedNodes();

        public override FieldInfo[] GetNodeFields()
        {
            FieldInfo[] fields = base.GetNodeFields();
            Array.Sort(fields, (f1, f2) => f1.Name == nameof(FlowOut) ? -1 : 1);
            return fields;
        }
    }

    public abstract class TooltipFactoryLinearNode : TooltipFactoryNode
    {
        [Input(name = "Flow", allowMultiple = true)]
        public TooltipFactoryLink FlowIn;

        public override IEnumerable<TooltipFactoryNode> GetExecutedNodes()
        {
            return outputPorts.FirstOrDefault(n => n.fieldName == nameof(FlowOut))
                ?.GetEdges().Select(e => e.inputNode as TooltipFactoryNode);
        }
    }

    [NodeMenuItem("Tooltip Factory/Start")]
    public class TooltipFactoryStartNode : TooltipFactoryNode
    {
        public override string name => "Start";

        [Output(name = "Item")]
        public ItemBase Item;

        public override IEnumerable<TooltipFactoryNode> GetExecutedNodes()
        {
            return outputPorts.FirstOrDefault(n => n.fieldName == nameof(FlowOut))
                ?.GetEdges().Select(e => e.inputNode as TooltipFactoryNode);
        }

        protected override void Process()
        {
            base.Process();
            Item = ((TooltipFactoryGraph)graph).Item;
        }
    }

    [NodeMenuItem("Tooltip/Content/Text")]
    public class TooltipFactoryAddTextNode : TooltipFactoryLinearNode
    {
        public override string name => "Text";

        [Input, SerializeField] public string Text;
        [Input, SerializeField, ColorUsage(true)] public Color Color = Color.white;
        [Input, SerializeField] public FontWeight FontWeight = FontWeight.Regular;
        [Input, SerializeField] public float FontSize = 28;
        [Input, SerializeField] public FontStyles FontStyle = FontStyles.Bold;

        protected override void Process()
        {
            base.Process();
            GameObject obj = UnityEngine.Object.Instantiate(Manager.Use<InventoryManager>().BasicTextPrefab, ((TooltipFactoryGraph)graph).TooltipRoot.transform);
            TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
            text.text = Text;
            text.fontWeight = FontWeight;
            text.fontSize = FontSize;
            text.color = Color;
            text.fontStyle = FontStyle;
        }
    }

    [NodeMenuItem("Tooltip/Item/Split")]
    public class TooltipFactorySplitItemNode : BaseNode
    {
        public override string name => "Split Item";

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

    [NodeMenuItem("Tooltip/Item/Split")]
    public class TooltipFactorySplitRarityNode : BaseNode
    {
        public override string name => "Split Rarity";

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