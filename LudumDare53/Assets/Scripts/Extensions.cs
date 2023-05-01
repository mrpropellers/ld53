using System.Linq;
using UnityEngine;

namespace LeftOut.LudumDare
{
    public static class Extensions 
    {
        public static Color GetColor(this string hex)
        {
            if (!hex.Contains("#"))
            {
                Debug.LogError("Hex string must contain hash #!");
            }
            return ColorUtility.TryParseHtmlString(hex, out var colorFromHex) ? colorFromHex : Color.black;
        }
    }
}
