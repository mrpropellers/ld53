using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LeftOut.LudumDare
{
    [Serializable]
    public class Pollen
    {
        public enum ColorNames { Red, Yellow, Blue, Green, Orange, Violet };
        public static readonly Dictionary<ColorNames, Color> NameToColor = new()
        {
            { ColorNames.Red, Color.red }, { ColorNames.Yellow, Color.yellow }, { ColorNames.Blue,Color.blue },
            { ColorNames.Green, Color.green}, { ColorNames.Orange, new Color(0.89f, 0.53f, 0.04f) }, 
            { ColorNames.Violet, new Color(0.63f, 0.26f, 0.96f) }
        };
        public Flower Parent { get; private set; }
        public Color Color { get; private set; }

        public Pollen(Color color, Flower parent)
        {
            Color = color;
            Parent = parent;
        }

        public static bool VerifyPollination(Pollen orig, Pollen incoming)
        {
            var origName = NameToColor.FirstOrDefault(x => x.Value.Equals(orig.Color)).Key;
            var incomingName = NameToColor.FirstOrDefault(x => x.Value.Equals(incoming.Color)).Key;
            switch (origName)
            {
                case ColorNames.Red when incomingName == ColorNames.Yellow:
                case ColorNames.Yellow when incomingName == ColorNames.Red:
                case ColorNames.Yellow when incomingName == ColorNames.Blue:
                case ColorNames.Blue when incomingName == ColorNames.Yellow:
                case ColorNames.Blue when incomingName == ColorNames.Red:
                case ColorNames.Red when incomingName == ColorNames.Blue:
                    return true;
                case ColorNames.Green:
                case ColorNames.Orange:
                case ColorNames.Violet:
                default:
                    return false;
            }
        }
    }
}
