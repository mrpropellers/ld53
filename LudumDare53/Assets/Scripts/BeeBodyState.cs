using System;
using UnityEngine;

namespace LeftOut.LudumDare
{
    public class BeeBodyState : MonoBehaviour
    {
        Pollen m_Pollen;

        public bool HasPollen => m_Pollen != null;
        public Flower PollenSource => m_Pollen.Parent;

        public Pollen YieldPollen()
        {
            if (m_Pollen == null)
            {
                Debug.LogError($"No pollen to yield! (check {nameof(HasPollen)} first)");
            }
            var p = m_Pollen;
            m_Pollen = null;
            return p;
        }

        public void CoverSelf(Pollen pollen)
        {
            if (m_Pollen != null)
            {
                Debug.LogWarning($"Overwriting old pollen! ({m_Pollen})");
            }

            m_Pollen = pollen;
        }
    }
}
