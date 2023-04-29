using System;
using UnityEngine;

namespace LeftOut.JamAids
{
    public class Unbreakable : MonoBehaviour, IDamageable
    {
        DamageResult m_DamageResult;

        void Awake()
        {
            // Result is always the same, so we can just cache it
            m_DamageResult = new DamageResult(0);
        }

        public DamageResult ProcessDamage(DamageAttempt attempt)
        {
            return m_DamageResult;
        }
    }
}
