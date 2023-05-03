using System.Linq;
using UnityEngine;

namespace LeftOut.LudumDare
{
    public static class Extensions
    {
        static readonly Color s_Black = 
            ColorUtility.TryParseHtmlString("#323232", out var color) ? color : Color.magenta;
        
        public static Color GetColor(this string hex)
        {
            if (!hex.Contains("#"))
            {
                Debug.LogError("Hex string must contain hash #!");
            }

            // Sneaky chance to get a black flower
            if (Random.value > 0.992f)
            {
                return s_Black;
            }
            return ColorUtility.TryParseHtmlString(hex, out var colorFromHex) ? colorFromHex : Color.magenta;
        }
    }
}
