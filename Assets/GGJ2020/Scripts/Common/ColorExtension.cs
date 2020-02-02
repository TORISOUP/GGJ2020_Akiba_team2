using UnityEngine;

namespace GGJ2020.Common
{
    public static class ColorExtension
    {
        public static Color SetA(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}