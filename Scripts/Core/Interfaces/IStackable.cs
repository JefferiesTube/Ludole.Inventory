namespace Ludole.Inventory
{
    public interface IStackable
    {
        int StackSize { get; set; }
        int MaxStackSize { get; set; }
    }
}