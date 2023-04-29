using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeftOut.LudumDare
{
    public class PollinationSoundController : MonoBehaviour
    {
        public FMODUnity.StudioEventEmitter emitter;

        void Start()
        {
            emitter = GetComponent<FMODUnity.StudioEventEmitter>();

        }

        public void PollinationSound()
        {
            emitter.Play();
        }
    }
}
