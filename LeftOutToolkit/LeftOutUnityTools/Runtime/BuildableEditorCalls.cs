#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Assertions;
using LeftOut.Extensions;
using UnityEngine;

namespace LeftOut
{
    public static class EditorAgnostic
    {
        public static void Destroy(GameObject obj)
        {
#if UNITY_EDITOR
            if (obj.IsAssetOnDisk())
            {
                Debug.Log($"Refusing to destroy {obj.name}");
                return;
            }

            if (Application.isPlaying)
            {
                Object.Destroy(obj);
            }
            else
            {
                Object.DestroyImmediate(obj);
            }
#else
            Assert.IsTrue(Application.isPlaying);
            Object.Destroy(obj);
#endif
        }

        public static void Destroy(MonoBehaviour monoBehaviour) => Destroy(monoBehaviour.gameObject);
    }
}
