using UnityEngine;

namespace LeftOut.Extensions
{
    public static class TransformExtensions
    {
        public static Bounds ReprojectBounds(this Transform destination, Bounds bounds, Transform source)
        {
            var pts = new Vector3[8];
            var ext = bounds.extents;
            var c = bounds.center;
            pts[0] = c + Vector3.Scale(ext, new Vector3(-1, -1, -1));
            pts[1] = c + Vector3.Scale(ext, new Vector3(-1, -1, 1));
            pts[2] = c + Vector3.Scale(ext, new Vector3(-1, 1, -1));
            pts[3] = c + Vector3.Scale(ext, new Vector3(1, -1, -1));
            pts[4] = c + Vector3.Scale(ext, new Vector3(1, 1, -1));
            pts[5] = c + Vector3.Scale(ext, new Vector3(1, -1, 1));
            pts[6] = c + Vector3.Scale(ext, new Vector3(-1, 1, 1));
            pts[7] = c + Vector3.Scale(ext, new Vector3(1, 1, 1));
            var max = Vector3.negativeInfinity;
            var min = Vector3.positiveInfinity;
            for (var i = 0; i < 8; i++)
            {
                pts[i] = source.TransformPoint(pts[i]);
                pts[i] = destination.InverseTransformPoint(pts[i]);
                max = Vector3.Max(max, pts[i]);
                min = Vector3.Min(min, pts[i]);
            }

            var center = Vector3.Lerp(min, max, 0.5f);
            return new Bounds(center, max - min);
        }
    }
}
