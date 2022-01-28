using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ludole.Inventory.Editor
{
    [CustomEditor(typeof(Rarity))]
    public class RarityEditor : UnityEditor.Editor
    {
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            Rarity r = (Rarity) target;
            Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
            tex.SetPixels(Enumerable.Repeat(r.Color, width * height).ToArray());
            tex.Apply();
            return tex;
        }
    }
}