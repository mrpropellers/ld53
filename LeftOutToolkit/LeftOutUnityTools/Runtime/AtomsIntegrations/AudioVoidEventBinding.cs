using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace LeftOut.Atoms
{
    public class AudioVoidEventBinding : MonoBehaviour
    {
        [System.Serializable]
        class EventClipBinding
        {
            int m_FrameLastFired = int.MinValue;
            int m_ClipIndex = 0;
            public VoidEvent Event;
            [FormerlySerializedAs("AudioSource")]
            public AudioSource Source;
            public AudioClip[] AudioClips;
            [Range(0f, 2f)]
            public float BaseVolume = 1f;
            [Range(0f, 0.5f)]
            public float VolumeVariance;
            [Range(0f, 4f)]
            public float BasePitch = 1f;
            [Range(0f, 2.0f)]
            public float PitchVariance;
            // Affects how frequently this audio event is allowed to fire
            [Range(0, 60)]
            public int CooldownFrames = 1;

            public bool IsInCooldown => m_FrameLastFired + CooldownFrames > Time.frameCount;

            AudioClip NextClip
            {
                get
                {
                    var clip = AudioClips[m_ClipIndex];
                    m_ClipIndex++;
                    if (m_ClipIndex >= AudioClips.Length)
                    {
                        m_ClipIndex = 0;
                    }

                    return clip;
                }
            }

            public void Fire()
            {
                Debug.Assert(Source != null);
                Source.pitch =
                    BasePitch + Random.Range(-PitchVariance, PitchVariance);
                Source.PlayOneShot(NextClip,
                    BaseVolume + Random.Range(-VolumeVariance, VolumeVariance));
                m_FrameLastFired = Time.frameCount;
            }
        }

        [SerializeField]
        EventClipBinding[] m_Bindings;

        void Awake()
        {
            var numAdded = 0;
            for (var i = 0; i < m_Bindings.Length; i++)
            {
                var binding = m_Bindings[i];
                if (binding.Event == null || binding.AudioClips.Length == 0)
                    continue;
                var clipNum = i;
                binding.Event.Register(() => PlayClip(clipNum));
                numAdded++;
            }

            if (numAdded < m_Bindings.Length)
            {
                Debug.LogWarning($"Only added {numAdded} of {m_Bindings.Length} bindings", this);
            }

        }

        void PlayClip(int bindingIndex)
        {
            if (m_Bindings[bindingIndex].IsInCooldown) return;

            m_Bindings[bindingIndex].Fire();
        }
    }
}
