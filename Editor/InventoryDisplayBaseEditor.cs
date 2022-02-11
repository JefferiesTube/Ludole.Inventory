using MarkupAttributes.Editor;
using UnityEditor;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(InventoryDisplayBase), true), CanEditMultipleObjects]
    public class InventoryDisplayBaseEditor : MarkedUpEditor
    {
    }
}