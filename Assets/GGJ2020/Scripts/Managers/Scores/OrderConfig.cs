using System;
using UnityEditor;
using UnityEngine;

namespace GGJ2020.Managers.Scores
{
    [CreateAssetMenu(menuName = "Create Order Config")]
    [Serializable]
    public class OrderConfig : ScriptableObject
    {
        public Order[] Orders;
    }
}