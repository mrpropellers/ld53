using UnityEngine;

namespace LeftOut.UnityMath
{
    public static class RandomNumbers
    {
        public static float FromNeg1To1()
        {
            return Random.Range(-1f, 1f);
        }
    }

}
