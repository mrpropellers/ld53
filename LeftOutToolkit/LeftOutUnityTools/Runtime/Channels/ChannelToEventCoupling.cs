using System;
using UnityEngine;
using UnityEngine.Events;

namespace LeftOut.GameplayManagement
{
    public class ChannelToEventCoupling : MonoBehaviour
    {
        [field:SerializeField]
        public UnityEvent Event { get; private set; }
        [field: SerializeField]
        public EventChannel Channel { get; private set; }

        void Start()
        {
            Channel.OnEvent.AddListener(() => Event?.Invoke());
        }
    }
}
