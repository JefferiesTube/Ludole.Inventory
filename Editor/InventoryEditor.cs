using MarkupAttributes.Editor;
using UnityEditor;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(Inventory), true), CanEditMultipleObjects]
    public class InventoryEditor : MarkedUpEditor
    {
    }
}