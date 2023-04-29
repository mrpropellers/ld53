using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LeftOut
{
    public class DamageAttempt : System.EventArgs
    {
        float m_RawDamage;
        int m_FrameHitboxSet;
        int m_FrameHurtboxSet;

        public GameObject Source { get; }
        public float HitboxMultiplier { get; private set; } = 1f;
        public float HurtboxMultiplier { get; private set; } = 1f;

        public float FinalDamageAmount => m_RawDamage * HitboxMultiplier * HurtboxMultiplier;

        public DamageAttempt(GameObject source, float damageAmount)
        {
            Source = source;
            m_RawDamage = damageAmount;
        }

        public void SetMultiplier(Hitbox hitbox)
        {
            if (Time.frameCount == m_FrameHitboxSet)
            {
                Debug.LogWarning($"[{Time.frameCount}] Setting hitbox multiplier twice on same frame -- " +
                    $"old value of {HitboxMultiplier} will be over-written with {hitbox.DamageMultiplier}");
            }

            m_FrameHitboxSet = Time.frameCount;
            HitboxMultiplier = hitbox.DamageMultiplier;
        }

        public void SetMultiplier(Hurtbox hurtbox)
        {
            if (Time.frameCount == m_FrameHurtboxSet)
            {
                Debug.LogWarning($"[{Time.frameCount}] Setting hitbox multiplier twice on same frame -- " +
                    $"old value of {HurtboxMultiplier} will be over-written with {hurtbox.DamageMultiplier}");
            }

            m_FrameHurtboxSet = Time.frameCount;
            HurtboxMultiplier = hurtbox.DamageMultiplier;
        }
    }

    public class DamageResult : System.EventArgs
    {
        // This doesn't mean that AmountApplied is non-zero, just that the attempt was not discarded
        public bool AttemptWasProcessed { get; private set; }
        public int AmountApplied;

        public static DamageResult Ignored = new DamageResult();

        DamageResult()
        {
            AmountApplied = int.MinValue;
            AttemptWasProcessed = false;
        }

        public DamageResult(int amountApplied)
        {
            AttemptWasProcessed = true;
            Debug.Assert(amountApplied >= 0);
            AmountApplied = amountApplied;
        }
    }

    public interface IDamageable
    {
        public DamageResult ProcessDamage(DamageAttempt attempt);
    }
}
