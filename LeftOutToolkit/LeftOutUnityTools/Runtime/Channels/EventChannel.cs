using System;
using UnityEngine;
using UnityEngine.Events;

namespace LeftOut
{
    [CreateAssetMenu(fileName = "EventChannel", menuName = "Left Out/Event Channel", order = 0)]
    public class EventChannel : ScriptableObject
    {
        [SerializeField]
        bool m_DEBUG_Invoke;

		[field: SerializeField]
        public UnityEvent OnEvent;

        void OnValidate()
        {
            if (m_DEBUG_Invoke)
            {
                OnEvent?.Invoke();
                m_DEBUG_Invoke = false;
            }
        }

        public void Invoke()
        {
            OnEvent?.Invoke();
        }
    }
}
