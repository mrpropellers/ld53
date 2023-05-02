using UnityEngine;

namespace LeftOut.LudumDare
{
    [CreateAssetMenu]
    public class AudioOptions : ScriptableObject
    {
        public void SetDefaults()
        {
            SetMusic(100f);
            SetSFX(75f);
        }
        
        public void SetMusic(float value)
        {
            Debug.Log($"Setting music to {value}");
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("MusicVolume", value);
        }

        public void SetSFX(float value)
        {
            Debug.Log($"Setting sfx to {value}");
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("SFXVolume", value);
        }
    }
}
