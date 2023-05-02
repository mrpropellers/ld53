using System;
using System.Collections;
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

        void Start()
        {
            m_ChildImages = GetComponentsInChildren<Image>();
            m_ChildTextMesh = GetComponentsInChildren<TextMeshProUGUI>();
            AudioOptions.SetDefaults();
        }

        public void FadeOut()
        {
            DOVirtual.Float(1f, 0f, FadeTime, SetAlphas);
            StartCoroutine(DisableAfter(FadeTime));
        }

        IEnumerator DisableAfter(float time)
        {
            yield return new WaitForSeconds(time);
            gameObject.SetActive(false);
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
