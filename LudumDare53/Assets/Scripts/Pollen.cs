using UnityEngine;

namespace LeftOut.LudumDare
{
    public struct Pollen
    {
        public Color Color { get; private set; }

        public Pollen(Color color)
        {
            Color = color;
        }
    }
}
