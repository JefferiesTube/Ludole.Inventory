using UnityEditor;
using UnityEditor.Sprites;
using UnityEngine;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(ItemBase), true)]
    public class ItemEditor : UnityEditor.Editor
    {
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            ItemBase item = (ItemBase)target;
            if (item.Visual == null)
                return base.RenderStaticPreview(assetPath, subAssets, width, height);

            Texture2D sprite = SpriteUtility.GetSpriteTexture(item.Visual, false);
            Texture2D cache = new Texture2D(width, height, TextureFormat.ARGB32, true);
            EditorUtility.CopySerialized(sprite, cache);
            return cache;
        }
    }
}