using UnityEngine;
using static UnityEditor.PrefabUtility;

namespace LeftOut.Editor
{
    public static class EditorHelpers
    {
        public static bool IsAPrefab(GameObject go)
        {
            // It wasn't instantiated from a prefab...
            return GetCorrespondingObjectFromSource(go) == null 
                // ... but it is still a prefab instance
                && GetPrefabInstanceHandle(go) != null; 
        }
    }
}
