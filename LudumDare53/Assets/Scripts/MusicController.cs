using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeftOut.LudumDare
{
    public class MusicController : MonoBehaviour
    {
        // height of the camera, currently maxes out at 100
        public float height;
        private const float HEIGHT_MAX = 100;

        // level of bloom completion, maxes out at 100
        public float bloom;


        //Music event emitter
        public FMODUnity.StudioEventEmitter Music;

        private void Start()
        {
            Music = GetComponent<FMODUnity.StudioEventEmitter>();

        }

        // continuously update music
        private void Update()
        {
            Music.EventInstance.setParameterByName("Bloom", bloom);
            Music.EventInstance.setParameterByName("Proximity", height);
        }

    }

    
}
