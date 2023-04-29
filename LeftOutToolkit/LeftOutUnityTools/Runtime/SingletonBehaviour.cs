using UnityEngine;

namespace LeftOut
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        class TypeWrapper
        {
            // Ensures compiler won't mark this type as BeforeFieldInit
            // https://csharpindepth.com/articles/BeforeFieldInit
            static TypeWrapper() { }

            internal static T instance;
        }

        public static T Instance => TypeWrapper.instance;

        protected virtual void Awake ()
        {
            if (Instance == this)
            {
                // Already loaded, likely not destroyed during Scene load (which we'll assume is ok)
                return;
            }
            if (Instance != null)
            {
                Debug.LogError(
                    $"{Instance.name} and {name} are instances of the same Singleton type - please fix!");
            }
            TypeWrapper.instance = (T)this;
        }

        protected virtual void OnDestroy () {
            if (TypeWrapper.instance == this) {
                TypeWrapper.instance = null;
            }
        }
    }
}
