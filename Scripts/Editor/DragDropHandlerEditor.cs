using MarkupAttributes.Editor;
using UnityEditor;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(DragDropHandler), true), CanEditMultipleObjects]
    public class DragDropHandlerEditor : MarkedUpEditor
    {
    }
}