using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeftOut.LudumDare
{
    public class BeeBuzzSound : MonoBehaviour
    {
        public FMODUnity.StudioEventEmitter buzz;

        // buzz intensity parameter
        public float buzzSound = 10;

        // Start is called before the first frame update
        void Start()
        {
            buzz = GetComponent<FMODUnity.StudioEventEmitter>();
        }

        private void Update()
        {
            buzz.EventInstance.setParameterByName("Buzz", buzzSound);
        }

    }
}
