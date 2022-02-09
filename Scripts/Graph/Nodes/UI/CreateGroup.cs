using System;
using GraphProcessor;
using Ludole.Core;
using UnityEngine;

namespace Ludole.Inventory
{
    [NodeMenuItem("UI/Group")]
    public class CreateGroup : FlowNode
    {
        public enum GroupType
        {
            Horizontal,
            Vertical
        }

        public override string name => "Create UI Group";

        [Output] public UIGroup Group;
        [SerializeField, Setting("Type")] public GroupType Type;

        protected override void Process()
        {
            base.Process();
            RectTransform parent = (RectTransform)((TooltipGraph)graph).TooltipRoot.transform;
            Group = new UIGroup();
            switch (Type)
            {
                case GroupType.Horizontal:
                    Group.Parent = UnityEngine.Object
                        .Instantiate(Manager.Use<TooltipManager>().HorizontalGroupPrefab, parent)
                        .GetComponent<RectTransform>();
                    break;
                case GroupType.Vertical:
                    Group.Parent = UnityEngine.Object
                        .Instantiate(Manager.Use<TooltipManager>().VerticalGroupPrefab, parent)
                        .GetComponent<RectTransform>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}