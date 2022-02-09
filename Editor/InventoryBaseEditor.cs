using MarkupAttributes.Editor;
using UnityEditor;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(InventoryBase), true), CanEditMultipleObjects]
    public class InventoryBaseEditor : MarkedUpEditor
    {
    }
}