using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LeftOut.UnityMath
{
    public static class Geometry
    {
        // https://stackoverflow.com/a/56794499
        static void ComputeQuaternionSample(out float i, out float j, out float r)
        {
            const int maxAllowedTries = 1000;
            var numTries = 0;
            do
            {
                i = RandomNumbers.FromNeg1To1();
                j = RandomNumbers.FromNeg1To1();
                r = i * i + j * j;
            } while (r > 1 && ++numTries < maxAllowedTries);

            if (numTries == maxAllowedTries)
            {
                Debug.LogError($"Failed to compute valid random Quaternion after {numTries} tries.");
            }
        }

        public static Quaternion ConstructRandomQuaternion()
        {
            ComputeQuaternionSample(out var x, out var y, out var z);
            ComputeQuaternionSample(out var u, out var v, out var w);
            var s = Mathf.Sqrt((1f - z) / w);
            return new Quaternion(x, y, s * u, s * v);
        }
    }
}
