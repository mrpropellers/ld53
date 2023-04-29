using UnityEngine;

namespace LeftOut
{
    public class DamagePasserOncePerFrame : MonoBehaviour, IDamageable
    {
        int m_LastFrameDamaged = int.MinValue;

        // We don't make this an event because it's just raw data passing - downstream handlers can decide
        // when to raise UnityEvents based on whether or not the damage resolves, etc.
        public System.Func<DamageAttempt, DamageResult> OnDamageAttempt { get; set; }

        public bool TryDamage(GameObject source, int amount)
        {
            return ProcessDamage(new DamageAttempt(source, amount)).AttemptWasProcessed;
        }

        public DamageResult ProcessDamage(DamageAttempt attempt)
        {
            if (m_LastFrameDamaged == Time.frameCount || OnDamageAttempt == null)
            {
                var sourceName = attempt.Source == null ? "null" : attempt.Source.name;
                Debug.Log(
                    $"Already received damage from a source this frame, " +
                    $"ignoring damage from {sourceName}");
                return DamageResult.Ignored;
            }

            m_LastFrameDamaged = Time.frameCount;
            return OnDamageAttempt.Invoke(attempt);
        }
    }
}
