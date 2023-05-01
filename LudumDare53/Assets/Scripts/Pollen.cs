using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LeftOut.LudumDare
{
    [Serializable]
    public class Pollen
    {
        public enum ColorNames { Purple, Pink, Red, Orange, Yellow, White };
        public static string[] HexList = { "#AC92EC", "#EC87C0", "#ED5565", "#FC6E51", "#FFCE54", "#F5F7FA" };

        public Dictionary<ColorNames, string> ColorNameToHex = new Dictionary<ColorNames, string>()
        {
            {ColorNames.Purple, "#AC92EC"}, {ColorNames.Pink, "#EC87C0"}, {ColorNames.Red, "#ED5565"}, {ColorNames.Orange, "#FC6E51"},
            {ColorNames.Yellow, "#FFCE54"}, {ColorNames.White, "#F5F7FA"}
        };
        public Flower Parent { get; set; }
        public Color Color { get; private set; }

        public Pollen(Color color, Flower parent)
        {
            Color = color;
            Parent = parent;
        }
    }
}
