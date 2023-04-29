using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace LeftOut
{
    public class DoNotDestroy : MonoBehaviour
    {
        [SerializeField]
        IntReference m_InstanceCounter;

        void Awake()
        {
            if (m_InstanceCounter.Value > 1)
            {
                Debug.LogError($"{m_InstanceCounter.ToString()} shows more than one already existing " +
                    $"(shouldn't be possible)");
            }

            if (m_InstanceCounter.Value > 0)
            {
                Debug.Log($"An instance of {name} already exists so this one will self-destruct");
                Destroy(gameObject);
            }
            else
            {
                m_InstanceCounter.Value = 1;
                DontDestroyOnLoad(this);
            }
        }
    }
}
