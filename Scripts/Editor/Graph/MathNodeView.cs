﻿
using UnityEngine;
using UnityEngine.UIElements;
using BlueGraph;
using BlueGraph.Editor;
using UnityEditor;
using System;

namespace Ludole.Inventory
{
    /// <summary>
    /// Base view for math nodes
    /// </summary>
    [CustomNodeView(typeof(MathNode))]
    public class MathNodeView : NodeView
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();

            styleSheets.Add(Resources.Load<StyleSheet>("MathNodeView"));
            AddToClassList("mathNodeView");
        }
    }
}
