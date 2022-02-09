using MarkupAttributes.Editor;
using UnityEditor;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(DragDropManager), true), CanEditMultipleObjects]
    public class DragDropHandlerEditor : MarkedUpEditor
    {
    }
}