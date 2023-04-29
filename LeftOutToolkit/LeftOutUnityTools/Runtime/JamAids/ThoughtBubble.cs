using UnityEngine;

namespace LeftOut.JamAids
{
    public class ThoughtBubble : MonoBehaviour
    {
        [SerializeField]
        AnimationCurve m_BounceCurve;

        [SerializeField]
        float m_BounceAmplitude;

        [SerializeField]
        float m_BounceFrequency;

        Vector3 m_ThisInitialPositionLocal;

        [SerializeField]
        SpriteRenderer m_DesireRenderer;

        [SerializeField]
        bool m_IsBouncing = true;
        
        float m_FloatingAwayDisplacement;

        public void SetDesire(Sprite item)
        {
            m_DesireRenderer.sprite = item;
            m_IsBouncing = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            m_IsBouncing = true;
            m_ThisInitialPositionLocal = transform.localPosition;
            //m_DesireInitialPositionLocal = m_ObjectOfDesire.transform.localPosition;
        }

        //public void FlyAway()
        //{
        //    m_IsBouncing = false;
        //    m_FloatingAwayDisplacement = 0;
        //}


        // Update is called once per frame
        void Update()
        {
            var bounceDisplacement = m_BounceCurve.Evaluate(Time.time * m_BounceFrequency) * m_BounceAmplitude;
            if (m_IsBouncing)
            {
                transform.localPosition = m_ThisInitialPositionLocal + bounceDisplacement * Vector3.up;
            }
            else
            {
                if (Mathf.Approximately(m_FloatingAwayDisplacement, 0))
                {
                    m_FloatingAwayDisplacement = bounceDisplacement;
                }
                else
                {
                    m_FloatingAwayDisplacement += Time.deltaTime * m_BounceAmplitude * m_BounceFrequency;
                }

                transform.localPosition = m_ThisInitialPositionLocal + m_FloatingAwayDisplacement * Vector3.up;
            }
        }
    }
}
