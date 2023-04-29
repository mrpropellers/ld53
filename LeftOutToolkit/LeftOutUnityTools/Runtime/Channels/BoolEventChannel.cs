using System;
using UnityEngine;
using UnityEngine.Events;

namespace LeftOut
{
    [CreateAssetMenu(fileName = "BoolEventChannel", menuName = "Left Out/Bool Event Channel", order = 0)]
    public class BoolEventChannel : ScriptableObject
    {
        [SerializeField]
        bool m_DEBUG_Invoke;
        [SerializeField]
        bool m_DEBUG_Value;

        [field: SerializeField]
        public UnityEvent<bool> OnEvent;

        void OnValidate()
        {
            if (m_DEBUG_Invoke)
            {
                OnEvent?.Invoke(m_DEBUG_Value);
                m_DEBUG_Invoke = false;
            }
        }
    }
}
