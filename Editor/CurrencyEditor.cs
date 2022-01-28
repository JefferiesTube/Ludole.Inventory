using System;
using System.Text;
using MarkupAttributes.Editor;
using UnityEditor;
using UnityEditor.Sprites;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(Currency), true)]
    public class CurrencyEditor : MarkedUpEditor
    {
        MarkupGUI.GroupsStack groupsStack = new MarkupGUI.GroupsStack();

        private ValueBreakdown[] _lastResult = Array.Empty<ValueBreakdown>();
        private long _testValue = 0;
        private static StringBuilder _builder = new StringBuilder();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            groupsStack.Clear();
            groupsStack += MarkupGUI.BeginTitleGroup("Testing");

           _testValue = EditorGUILayout.LongField("Input", _testValue);
           _lastResult = (target as Currency).Breakdown(_testValue);
           
           if (_lastResult.Length == 0)
           {
               EditorGUILayout.LabelField("No value");
           }
           else
           {
               _builder.Clear();
               for (int i = 0; i < _lastResult.Length; i++)
               {
                   _builder.Append($"{_lastResult[i].Value} {_lastResult[i].CurrencyUnit.Name}  ");
               }

               EditorGUILayout.LabelField(_builder.ToString());
           }
           groupsStack.EndAll();
        }

        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            Currency item = (Currency)target;
            if (item.Sprite == null)
                return base.RenderStaticPreview(assetPath, subAssets, width, height);

            Texture2D sprite = SpriteUtility.GetSpriteTexture(item.Sprite, false);
            Texture2D cache = new Texture2D(width, height, TextureFormat.ARGB32, true);
            EditorUtility.CopySerialized(sprite, cache);
            return cache;
        }
    }
}