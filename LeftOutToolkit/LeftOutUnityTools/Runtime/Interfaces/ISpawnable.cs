using UnityEngine;

namespace LeftOut
{
    public interface ISpawnable
    {
        public GameObject SpawnOne(Transform parent = null);
    }
}
