namespace Ludole.Inventory
{
    public interface IContainer
    {
        SlotInventory ReferencedSlotInventory { get; set; }
    }
}