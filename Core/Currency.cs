using System;
using System.Collections.Generic;
using System.Linq;
using Ludole.Core;
using MarkupAttributes;
using UnityEngine;

namespace Ludole.Inventory
{
    [Serializable]
    public class CurrencyUnit
    {
        [SerializeField] private string _name;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [SerializeField] private string _description;

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        [SerializeField, SpritePreview] private Sprite _sprite;

        public Sprite Sprite
        {
            get => _sprite;
            set => _sprite = value;
        }

        [SerializeField, Min(1)] private int _modulus = 1;

        public int Modulus
        {
            get => _modulus;
            set => _modulus = value;
        }
    }

    [Serializable]
    public struct ValueBreakdown
    {
        public CurrencyUnit CurrencyUnit;
        public long Value;
    }

    [CreateAssetMenu(fileName = "currency.asset", menuName = "Inventory/Currency")]
    public class Currency : ScriptableObject
    {
        [TitleGroup("General"), SerializeField] private string _name;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [SerializeField] private string _description;

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        [SerializeField, SpritePreview] private Sprite _sprite;

        public Sprite Sprite
        {
            get => _sprite;
            set => _sprite = value;
        }

        [TitleGroup("Definition"), SerializeField] private CurrencyUnit[] _units;

        public CurrencyUnit[] Units
        {
            get => _units;
            set => _units = value;
        }

        public ValueBreakdown[] Breakdown(long value)
        {
            List<ValueBreakdown> result = new List<ValueBreakdown>();
            if (_units == null)
                return result.ToArray();

            int sign = value >= 0 ? 1 : -1;
            value = Math.Abs(value);

            for (int i = Units.Length - 1; i >= 0; i--)
            {
                long fragment = i == 0 ? value : value % Units[i].Modulus;
                if (fragment > 0)
                {
                    result.Add(new ValueBreakdown { CurrencyUnit = Units[i], Value = fragment * sign });
                }

                value /= Units[i].Modulus == 0 ? 1 : Units[i].Modulus;
            }

            result.Reverse();
            return result.ToArray();
        }
    }
}