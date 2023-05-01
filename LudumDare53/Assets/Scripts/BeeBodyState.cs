using System;
using UnityEngine;
using UnityEngine.Events;

namespace LeftOut.LudumDare
{
    public class BeeBodyState : MonoBehaviour
    {
        Pollen m_Pollen;

        public bool HasPollen => m_Pollen != null;
        public Flower PollenSource => m_Pollen.Parent;

        public UnityEvent<BeeBodyState> StateChanged;

        void Start()
        {
            m_Pollen = null;
        }

        public Pollen YieldPollen()
        {
            if (m_Pollen == null)
            {
                Debug.LogError($"No pollen to yield! (check {nameof(HasPollen)} first)");
            }
            var p = m_Pollen;
            m_Pollen = null;
            StateChanged?.Invoke(this);
            return p;
        }

        public void CoverSelf(Pollen pollen)
        {
            if (m_Pollen != null)
            {
                Debug.LogWarning($"Overwriting old pollen! ({m_Pollen})");
            }

            m_Pollen = pollen;
            StateChanged?.Invoke(this);
        }
    }
}
