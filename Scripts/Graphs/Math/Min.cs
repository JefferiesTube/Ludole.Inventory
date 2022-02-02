﻿
using BlueGraph;
using UnityEngine;

namespace Ludole.Inventory
{
    [Node(Path = "Math/Comparison")]
    [Tags("Math")]
    public class Min : MathNode<float, float, float>
    {
        public override float Execute(float value1, float value2)
        {
            return Mathf.Min(value1, value2);
        }
    }
}