using Ludole.Core;
using UnityEngine;

namespace Ludole.Inventory
{
    public enum TooltipMode { FollowMouse, RelativeToSlot }

    public class TooltipManager : ManagerModule
    {
        public GameObject Tooltip;

        public TooltipMode TooltipMode;
        public Vector2 TooltipOffset = new Vector2(8, 8);

        public string[] TooltipIgnoreTags;

        public TooltipGraph TooltipGraph;

        [SerializeField] public GameObject BasicTextPrefab;
        public GameObject HorizontalGroupPrefab;
        public GameObject VerticalGroupPrefab;

        public override void OnAwake()
        {
            base.OnAwake();
            Tooltip.SetActive(false);
        }
    }
}
