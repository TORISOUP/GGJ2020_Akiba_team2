using System;
using System.Collections.Generic;
using UnityEngine;
using Type = GGJ2020.Parts.Type;

namespace GGJ2020.Managers.Scores
{
    public enum OrderName
    {
        Car,
        Robot,
        Doll
    }
    
    /// <summary>
    /// Partの組み合わせ
    /// </summary>
    [Serializable]
    public struct Order
    {
        [SerializeField] private OrderName _name;
        public OrderName Name => _name;
        [SerializeField] private int _comboBonus;
        [SerializeField] private OrderElement[] _elements;
        public IReadOnlyList<OrderElement> Elements => _elements;

        public int ComboBonus => _comboBonus;

        public Order(OrderName name, int comboBonus, OrderElement[] elements)
        {
            _name = name;
            _comboBonus = comboBonus;
            _elements = elements;
        }
    }

    [Serializable]
    public struct OrderElement : IEquatable<OrderElement>
    {
        [SerializeField] private Type _partType;
        [SerializeField] private int _needCount;
        public Type PartType => _partType;
        public int NeedCount => _needCount;

        public OrderElement(Type partType, int needCount)
        {
            _partType = partType;
            _needCount = needCount;
        }

        public bool Equals(OrderElement other)
        {
            return _partType == other._partType && _needCount == other._needCount;
        }

        public override bool Equals(object obj)
        {
            return obj is OrderElement other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) _partType * 397) ^ _needCount;
            }
        }
    }
}