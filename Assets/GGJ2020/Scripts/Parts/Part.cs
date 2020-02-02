using System;
using UnityEngine;

namespace GGJ2020.Parts
{
    [Serializable]
    public class Part
    {
        [SerializeField] private  Type _type;
        [SerializeField] private  Quality _quality;
        [SerializeField] private  int _unitScore;

        public Type Type => _type;
        public Quality Quality => _quality;
        public int UnitScore => _unitScore;

        public Part(Type type, Quality quality, int unitScore)
        {
            _type = type;
            _quality = quality;
            _unitScore = unitScore;
        }
    }
}