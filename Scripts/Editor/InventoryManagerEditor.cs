using MarkupAttributes.Editor;
using UnityEditor;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(InventoryManager), true), CanEditMultipleObjects]
    public class InventoryManagerEditor : MarkedUpEditor
    {
    }
}