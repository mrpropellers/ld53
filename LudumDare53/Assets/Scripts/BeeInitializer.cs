using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeftOut.LudumDare
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(BeeControlSwitcher))]
    [RequireComponent(typeof(BeeGroundController))]
    public class BeeInitializer : MonoBehaviour, IAtomListener
    {
        BeeControlSwitcher m_BeeControlState;
        BeeGroundController m_GroundController;

        [SerializeField]
        UnityAtoms.AtomEventBase GameStarted;

        void Awake()
        {
            m_BeeControlState = GetComponent<BeeControlSwitcher>();
            m_GroundController = GetComponent<BeeGroundController>();
        }

        void OnEnable()
        {
            GameStarted.RegisterListener(this);
        }

        void OnDisable()
        {
            GameStarted.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            m_BeeControlState.enabled = true;
            // We don't fully have our action map handling working right, so need to manually enable ground control
            m_GroundController.enabled = true;
        }
    }
}
