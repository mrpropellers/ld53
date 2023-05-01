using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LeftOut.LudumDare
{
    [Serializable]
    public class Pollen
    {
        public static string[] HexList = { "#AC92EC", "#EC87C0", "#ED5565", "#FC6E51", "#FFCE54", "#F5F7FA" };
        public Flower Parent { get; set; }
        public Color Color { get; private set; }

        public Pollen(Color color, Flower parent)
        {
            Color = color;
            Parent = parent;
        }
    }
}
