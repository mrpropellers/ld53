using System;
using System.Collections.Generic;
using LeftOut.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace LeftOut
{
    public class Hitbox : MonoBehaviour, IHaveOwner
    {
        Dictionary<GameObject,IDamageable> m_DamageReceivers;
        DamageAttempt m_DamageAttempt;

        public event EventHandler<DamageResult> OnDamageProcessed;

        [field: SerializeField]
        public GameObject Owner { get; internal set; }
        [field: SerializeField]
        // This is the base damage, modifiably only in Inspector and assumed const at Runtime
        public float DamageAmount { get; private set; } = 1f;
        // This is a non-const multiplier that anything can modify, but expected to be modified from script
        public float DamageMultiplier { get; set; } = 1f;

        public int CurrentDamage => Mathf.FloorToInt(DamageAmount * DamageMultiplier);

        [field: SerializeField]
        public bool IsOn { get; private set; }

        void Awake()
        {
            m_DamageReceivers = new Dictionary<GameObject, IDamageable>();
            m_DamageAttempt = new DamageAttempt(Owner, DamageAmount);
            // TODO: Remove -- this is just to fix bad initialization is editor cases
            DamageMultiplier = 1f;
        }

        void LateUpdate()
        {
            //if (!IsOn) return;

            m_DamageAttempt.SetMultiplier(this);
            // We only want to assign damage to any given receiver once per frame, even if we've had many collisions
            foreach (var damageable in m_DamageReceivers.Values)
            {
                if (!IsOn) break;
                var result = damageable.ProcessDamage(m_DamageAttempt);
                if (result.AttemptWasProcessed)
                {
                    OnDamageProcessed?.Invoke(this, result);
                    break;
                }
            }

            m_DamageReceivers.Clear();
        }

        void OnValidate()
        {
            m_DamageAttempt = new DamageAttempt(Owner, DamageAmount);
        }

        public void Activate()
        {
            m_DamageReceivers.Clear();
            IsOn = true;
        }

        public void Deactivate()
        {
            //Debug.Assert(m_IsWindingUp || m_IsOn);
            IsOn = false;
            //m_DamageReceivers.Clear();
        }

        bool AlreadyDamagingThisFrame(Collider col, Rigidbody rb) =>
            m_DamageReceivers.ContainsKey(col.gameObject)
                || rb != null && m_DamageReceivers.ContainsKey(rb.gameObject);

        bool AlreadyDamagingThisFrame(Collider2D col, Rigidbody2D rb) =>
            m_DamageReceivers.ContainsKey(col.gameObject)
                || rb != null && m_DamageReceivers.ContainsKey(rb.gameObject);

        void AssignDamage(IDamageable damageable)
        {
            var target = ((Component)damageable).gameObject;
            Debug.Assert(!m_DamageReceivers.ContainsKey(target),
                "Don't call AssignDamage on objects that will already be damaged this frame");
            if (!IsOn || !isActiveAndEnabled || m_DamageReceivers.ContainsKey(target))
                return;
            m_DamageReceivers.Add(target, damageable);
        }

        void AssignDamageIfDamageable(Collider col)
        {
            var rb = col.attachedRigidbody;
            if (AlreadyDamagingThisFrame(col, rb)) return;
            if (col.TryGetComponent(out IDamageable damageable)
                || rb != null && col.attachedRigidbody.TryGetComponent(out damageable))
            {
                AssignDamage(damageable);
            }
        }

        void AssignDamageIfDamageable(Collider2D col)
        {
            var rb = col.attachedRigidbody;
            if (AlreadyDamagingThisFrame(col, rb)) return;
            if (col.TryGetComponent(out IDamageable damageable)
                || rb != null && col.attachedRigidbody.TryGetComponent(out damageable))
            {
                AssignDamage(damageable);
            }
        }

        void OnCollisionEnter(Collision collision) => AssignDamageIfDamageable(collision.collider);

        void OnCollisionStay(Collision collisionInfo) => AssignDamageIfDamageable(collisionInfo.collider);

        void OnTriggerEnter(Collider other) => AssignDamageIfDamageable(other);

        void OnTriggerStay(Collider other) => AssignDamageIfDamageable(other);

        void OnCollisionEnter2D(Collision2D col) => AssignDamageIfDamageable(col.collider);

        void OnCollisionStay2D(Collision2D collision) => AssignDamageIfDamageable(collision.collider);

        void OnTriggerEnter2D(Collider2D col) => AssignDamageIfDamageable(col);

        void OnTriggerStay2D(Collider2D other) => AssignDamageIfDamageable(other);
    }
}
