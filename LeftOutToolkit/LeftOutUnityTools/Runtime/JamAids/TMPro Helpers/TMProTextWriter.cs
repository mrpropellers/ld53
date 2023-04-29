using System;
using TMPro;
using UnityEngine;

namespace LeftOut.JamAids
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public abstract class TMProTextWriter<T> : MonoBehaviour
    {
        TextMeshProUGUI m_GUI;

        protected virtual void Awake()
        {
            m_GUI = GetComponent<TextMeshProUGUI>();
        }

        public void Write(T value)
        {
            m_GUI.text = value.ToString();
        }
    }
}
