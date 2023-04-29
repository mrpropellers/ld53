using System.Collections.Generic;
using UnityEngine;

namespace LeftOut.Extensions
{
    public static class GameObjectExtensions
    {
        public static bool IsAssetOnDisk(this GameObject self) =>
            self.scene.rootCount == 0 || self.scene.name == null;

        public static bool TryGetComponentInParent<T>(this GameObject self, out T component) where T : Component
        {
            if (self.TryGetComponent(out component))
            {
                return true;
            }

            var parent = self.transform.parent;
            return parent != null && parent.gameObject.TryGetComponentInParent(out component);
        }

        public static T GetComponentInParent<T>(this GameObject self) where T : Component
        {
            return self.TryGetComponentInParent(out T component) ? component : null;
        }

        public static List<GameObject> GetGameObjectsInChildrenWithTag(this GameObject self, string tag)
        {
            var matches = new List<GameObject>();
            TraverseForTags(self.transform, tag, matches);
            return matches;
        }

        static void TraverseForTags(Transform node, string tag, List<GameObject> matches)
        {
            if (node.CompareTag(tag))
            {
                matches.Add(node.gameObject);
            }

            for (var i = 0; i < node.childCount; i++)
            {
                TraverseForTags(node.GetChild(i), tag, matches);
            }
        }
    }
}
