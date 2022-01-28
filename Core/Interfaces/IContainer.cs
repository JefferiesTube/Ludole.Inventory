namespace Ludole.Inventory
{
    public interface IContainer
    {
        Inventory ReferencedInventory { get; set; }
    }
}