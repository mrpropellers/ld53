using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LeftOut.JamAids
{
    [RequireComponent(typeof(BoundsSource))]
    public class ObjectSpawner : MonoBehaviour
    {
        BoundsSource m_Bounds;

        void Awake()
        {
            m_Bounds = GetComponent<BoundsSource>();
        }

        public GameObject SpawnRandomOrientation(GameObject toClone, Transform parent = null)
        {
            var spawnPoint = m_Bounds.GetRandomPoint();
            var orientation = UnityMath.Geometry.ConstructRandomQuaternion();
            var clone = Object.Instantiate(toClone, parent);
            clone.transform.SetPositionAndRotation(spawnPoint, orientation);
            return clone;
        }

        public GameObject SpawnRandomOrientation(ISpawnable spawnable, Transform parent = null)
        {
            var spawnPoint = m_Bounds.GetRandomPoint();
            var orientation = UnityMath.Geometry.ConstructRandomQuaternion();
            var spawned = spawnable.SpawnOne(parent);
            spawned.transform.SetPositionAndRotation(spawnPoint, orientation);
            return spawned.gameObject;
        }
    }
}
