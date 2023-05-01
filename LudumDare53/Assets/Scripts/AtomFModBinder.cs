using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms;
using UnityEngine;

namespace LeftOut.Atoms
{
    public class AtomFModBinder : MonoBehaviour, IAtomListener
    {
        [SerializeField]
        FMODUnity.StudioEventEmitter Emitter;
        [SerializeField]
        AtomEventBase AtomEvent;

        void OnValidate()
        {
            if (AtomEvent != null)
            {
                AtomEvent.RegisterListener(this);
            }
        }

        void OnEnable()
        {
            AtomEvent.RegisterListener(this);
        }

        void OnDisable()
        {
            AtomEvent.RegisterListener(this);
        }

        public void OnEventRaised()
        {
            Emitter.Play();
        }
    }
}
