using GraphProcessor;

namespace Ludole.Inventory
{
    public class TooltipConstructEventNode : EventNode
    {
        public override string name => "Start";

        [Output(name = "Item")] public ItemBase Item;

        protected override void Process()
        {
            base.Process();
            Item = ((TooltipGraph)graph).Item;
        }
    }
}