using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LeftOut.JamAids
{
    // Abstraction layer for the different types of objects which may return a bounding object
    public class BoundsSource : MonoBehaviour
    {
        [SerializeField]
        bool m_UseCollider;

        // TODO: Write custom Inspector so that only one source is visible
        [SerializeField]
        MeshFilter m_MeshSource;

        [SerializeField]
        Collider m_ColliderSource;

        // TODO: Switch on bounding type
        Transform BoundsTransform => m_UseCollider ? m_ColliderSource.transform : m_MeshSource.transform;

        public Bounds LocalBounds
        {
            get
            {
                if (m_UseCollider)
                {
                    // If collider is disabled - it will report Vector3.zero for its extents
                    var temporaryEnable = m_ColliderSource.enabled == false;
                    m_ColliderSource.enabled = true;
                    var bounds = m_ColliderSource.bounds;
                    m_ColliderSource.enabled = !temporaryEnable;
                    return bounds;
                }
                else
                {
                    return m_MeshSource.mesh.bounds;
                }
            }
        }

        void Awake()
        {
            m_UseCollider = TryGetComponent(out m_ColliderSource);
            m_MeshSource = GetComponent<MeshFilter>();
        }

        public Vector3 GetRandomPoint()
        {
            var tf = BoundsTransform;
            var extents = LocalBounds.extents;
            var pointLocal = new Vector3(
                Random.Range(-extents.x, extents.x),
                Random.Range(-extents.y, extents.y),
                Random.Range(-extents.z, extents.z));
            return tf.TransformPoint(pointLocal);
        }
    }
}
