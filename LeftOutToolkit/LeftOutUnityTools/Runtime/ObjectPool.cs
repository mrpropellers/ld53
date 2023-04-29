using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
//using LeftOut.Editor;
#endif

namespace LeftOut.Runtime
{
    // TODO: Add subclass for MonoBehaviour pool
    // Derives from ScriptableObject so we can access the OnDestroy event -- this might not be necessary...
    public class ObjectPool 
    {
        protected GameObject m_Original;
        Queue<GameObject> m_PoolInactive;
        HashSet<GameObject> m_TrackedInstances;
        
        public ObjectPool(GameObject goToPool)
        {
            m_Original = goToPool;
            m_PoolInactive = new Queue<GameObject>();
            m_TrackedInstances = new HashSet<GameObject>();
        }

        public GameObject GetOrInstantiate(Transform origin = null, string newName = "", bool shouldAttachToOrigin = false)
        {
            GameObject go;
            if (m_PoolInactive.Any())
            {
                go = m_PoolInactive.Dequeue();
            }
            else
            {
                go = GameObject.Instantiate(m_Original, origin);
                m_TrackedInstances.Add(go);
            }

            if (!newName.Equals(""))
            {
                go.name = newName;
            }

            go.transform.SetParent(shouldAttachToOrigin ? origin : null);

            go.SetActive(true);
            return go;
        }

        public void ReturnToPool(GameObject go)
        {
            if (!m_TrackedInstances.Contains(go))
            {
                throw new InvalidOperationException(
                    $"Can't return {go.name} to pool for {m_Original.name} because it was not created by this pool.");
            }

            go.SetActive(false);
            m_PoolInactive.Enqueue(go);
        }
    }
}