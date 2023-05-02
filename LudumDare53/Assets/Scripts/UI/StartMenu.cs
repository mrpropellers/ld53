using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LeftOut.LudumDare
{
    public class StartMenu : MonoBehaviour
    {
        Image[] m_ChildImages;
        TextMeshProUGUI[] m_ChildTextMesh;
        
        [SerializeField]
        float FadeTime = 2f;

        [SerializeField]
        AudioOptions AudioOptions;

        [SerializeField]
        CinemachineVirtualCamera PauseCamera;

        bool m_StillInStart;
        bool m_IsAnimating;
        bool m_IsPaused;

        void Start()
        {
            m_ChildImages = GetComponentsInChildren<Image>(includeInactive:true);
            m_ChildTextMesh = GetComponentsInChildren<TextMeshProUGUI>(includeInactive:true);
            AudioOptions.SetDefaults();
            m_StillInStart = true;
        }

        public void TogglePaused()
        {
            if (m_StillInStart)
            {
                Debug.Log("Can't pause yet.");
                return;
            }
            if (m_IsAnimating)
            {
                Debug.LogWarning("Cowardly refusing to handle this edge case.");
                return;
            }
            m_IsPaused = !m_IsPaused;
            PauseCamera.enabled = m_IsPaused;
            if (m_IsPaused)
            {
                gameObject.SetActive(true);
                StartCoroutine(FadeIn());
            }
            else
            {
                FadeOut();
            }
        }

        void OnEnable()
        {
            if (!m_StillInStart)
            {
                StartCoroutine(FadeIn());
            }
        }
        
        IEnumerator FadeIn()
        {
            m_IsAnimating = true;
            DOVirtual.Float(0f, 1f, FadeTime, SetAlphas);
            yield return new WaitForSeconds(FadeTime);
            m_IsAnimating = false;
        }

        public void FadeOut()
        {
            m_IsAnimating = true;
            DOVirtual.Float(1f, 0f, FadeTime, SetAlphas);
            StartCoroutine(DisableAfter(FadeTime));
        }

        IEnumerator DisableAfter(float time)
        {
            yield return new WaitForSeconds(time);
            gameObject.SetActive(false);
            m_IsAnimating = false;
            m_StillInStart = false;
        }
        

        void SetAlphas(float val)
        {
            foreach (var i in m_ChildImages)
            {
                var c = i.color;
                c.a = val;
                i.color = c;
            }

            foreach (var tmpro in m_ChildTextMesh)
            {
                tmpro.alpha = val;
            }
        }
    }
}
