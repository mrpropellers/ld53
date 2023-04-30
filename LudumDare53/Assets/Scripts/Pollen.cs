using UnityEngine;

namespace LeftOut.LudumDare
{
    public class Pollen
    {
        public Flower Parent { get; private set; }
        public Color Color { get; private set; }

        public Pollen(Color color, Flower parent)
        {
            Color = color;
            Parent = parent;
        }
    }
}
