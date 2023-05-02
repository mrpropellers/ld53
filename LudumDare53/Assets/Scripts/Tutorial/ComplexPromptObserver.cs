using System;
using UnityAtoms;
using UnityEngine;

namespace LeftOut.LudumDare
{
    public class ComplexPromptObserver : MonoBehaviour
    {
        bool m_HasFlown;
        bool m_HasLanded;
        
        [SerializeField]
        UnityAtoms.AtomEventBase FlowerSensedInFlight;
        [SerializeField]
        AtomEventBase HasLanded;
        [SerializeField]
        AtomEventBase GotPollen;
        [SerializeField]
        AtomEventBase CanPollinate;
        [SerializeField]
        AtomEventBase TookOff;

        BeeControlSwitcher FlightState;
        BeeBodyState BodyState;

        [SerializeField]
        UnityAtoms.AtomEventBase FlowerSensed;

        void Start()
        {
            m_HasFlown = FlightState.IsInFlight;
        }

        void OnEnable()
        {
            // TODO: Wire these up without using Finds (may want to utilize an InstanceTracker-type utility)
            FlightState = FindObjectOfType<BeeControlSwitcher>();
            BodyState = FindObjectOfType<BeeBodyState>();
            FlowerSensed.Register(OnFlowerSensed);
            FlightState.StateChanged.AddListener(OnFlyingStateChanged);
            BodyState.StateChanged.AddListener(OnBodyStateChanged);
        }

        void OnDisable()
        {
            FlowerSensed.Unregister(OnFlowerSensed);
            FlightState.StateChanged.RemoveListener(OnFlyingStateChanged);
            BodyState.StateChanged.RemoveListener(OnBodyStateChanged);
        }

        void OnFlyingStateChanged(BeeControlSwitcher handler)
        {
            if (handler.IsInFlight)
            {
                m_HasFlown = true;
                TookOff.Raise();
            }
            else if (m_HasFlown)
            {
                HasLanded.Raise();
                if (m_HasLanded && BodyState.HasPollen)
                {
                    CanPollinate.Raise();
                }
                m_HasLanded = true;
            }
        }

        void OnBodyStateChanged(BeeBodyState body)
        {
            if (body.HasPollen)
            {
                GotPollen.Raise();
            }
        }

        public void OnFlowerSensed()
        {
            if (FlightState.IsInFlight)
            {
                FlowerSensedInFlight.Raise();
            }
        }
    }
}
