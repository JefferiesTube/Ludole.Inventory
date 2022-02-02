﻿using UnityEngine;
using BlueGraph;

namespace Ludole.Inventory
{
    [Node(Path = "Math/Comparison")]
    [Tags("Math")]
    public class Equal : MathNode<float, float, bool>
    {
        public override bool Execute(float value1, float value2)
        {
            return Mathf.Approximately(value1, value2);
        }
    }
}
