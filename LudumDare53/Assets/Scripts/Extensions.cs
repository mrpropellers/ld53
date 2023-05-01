using System.Linq;
using UnityEngine;

namespace LeftOut.LudumDare
{
    public static class Extensions 
    {
        public static Pollen.ColorNames GetNameFromColor(this Pollen pollen)
        {
            return Pollen.NameToColor.FirstOrDefault(x => x.Value.Equals(pollen.Color)).Key;
        }
        
        public static bool VerifyPollination(this Pollen orig, Pollen incoming)
        {
            var origName = Pollen.NameToColor.FirstOrDefault(x => x.Value.Equals(orig.Color)).Key;
            var incomingName = Pollen.NameToColor.FirstOrDefault(x => x.Value.Equals(incoming.Color)).Key;
            switch (origName)
            {
                case Pollen.ColorNames.Red when incomingName == Pollen.ColorNames.Yellow:
                case Pollen.ColorNames.Yellow when incomingName == Pollen.ColorNames.Red:
                case Pollen.ColorNames.Yellow when incomingName == Pollen.ColorNames.Blue:
                case Pollen.ColorNames.Blue when incomingName == Pollen.ColorNames.Yellow:
                case Pollen.ColorNames.Blue when incomingName == Pollen.ColorNames.Red:
                case Pollen.ColorNames.Red when incomingName == Pollen.ColorNames.Blue:
                    return true;
                case Pollen.ColorNames.Green:
                case Pollen.ColorNames.Orange:
                case Pollen.ColorNames.Violet:
                default:
                    return false;
            }
        }
        
        // TODO: ???
        public static Pollen CrossPollinate(this Flower parent, Pollen orig, Pollen incoming)
        {
            var origName = Pollen.NameToColor.FirstOrDefault(x => x.Value.Equals(orig.Color)).Key;
            var incomingName = Pollen.NameToColor.FirstOrDefault(x => x.Value.Equals(incoming.Color)).Key;
            Debug.Log($"received orig {orig.Color} {origName} and {incoming.Color} {incomingName}");
            var color = orig.Color; 
            switch (origName)
            {
                case Pollen.ColorNames.Red when incomingName == Pollen.ColorNames.Yellow:
                case Pollen.ColorNames.Yellow when incomingName == Pollen.ColorNames.Red:
                    color = Pollen.NameToColor[Pollen.ColorNames.Orange];
                    break;
                case Pollen.ColorNames.Yellow when incomingName == Pollen.ColorNames.Blue:
                case Pollen.ColorNames.Blue when incomingName == Pollen.ColorNames.Yellow:
                    color = Pollen.NameToColor[Pollen.ColorNames.Green];
                    break;
                case Pollen.ColorNames.Blue when incomingName == Pollen.ColorNames.Red:
                case Pollen.ColorNames.Red when incomingName == Pollen.ColorNames.Blue:
                    color = Pollen.NameToColor[Pollen.ColorNames.Violet];
                    break;
                case Pollen.ColorNames.Green:
                case Pollen.ColorNames.Orange:
                case Pollen.ColorNames.Violet:
                default:
                    Debug.LogError($"Invalid color combo for {orig.Color} and {incoming.Color}");
                    break;
            }
            return new Pollen(color, parent);
        }
    }
}
