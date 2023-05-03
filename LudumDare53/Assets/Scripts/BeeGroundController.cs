using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeftOut.LudumDare
{
    [RequireComponent(typeof(BeeBodyState))]
    public class BeeGroundController : MonoBehaviour
    {
        public const string k_ActionMapName = "BeeLanded";
        
        Flower m_CurrentFlower;
        InputActionMap m_GroundedActions;
        
        [SerializeField]
        ParticleSystem m_ParticleSystem;

        ParticleSystem.MainModule m_ParticleMain;
        ParticleSystem.EmissionModule m_ParticleEmission;

        [SerializeField]
        BeeBodyState BodyState;
        
        [SerializeField]
        Transform BeeRoot;

        [SerializeField]
        InputActionReference GroundMoveAction;

        [SerializeField]
        float YawVelocity = 50f;
        [SerializeField]
        Animation PollinationDance;
        [SerializeField]
        ConstantLookAt BeeLookOverride;

        [SerializeField]
        UnityAtoms.AtomEventBase SuccessfulPollination;

        public bool DidPollinate { get; set; } = false;

        void Start()
        {
            m_GroundedActions = new InputActionMap(k_ActionMapName);
        }

        bool CanPollinate(Flower flower)
        {
            return BodyState.HasPollen && BodyState.PollenSource != flower;
        }

        void Start()
        {
            m_ParticleMain = m_ParticleSystem.main;
            m_ParticleEmission = m_ParticleSystem.emission;
        }

        void Update()
        {
            var rotation = GroundMoveAction.action.ReadValue<Vector2>().x;
            rotation *= Time.deltaTime * YawVelocity;
            BeeRoot.transform.Rotate(0, rotation, 0);
        }

        public void OnPollinate(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            if (m_CurrentFlower == null)
            {
                Debug.LogError("Current flower is null!");
                return;
            }
    
            m_GroundedActions.Disable();
            BeeLookOverride.enabled = false;
            StartCoroutine(AnimatePollinate());
        }

        IEnumerator AnimatePollinate()
        {
            Debug.Log("starting pollination dance");
            PollinationDance.Play();
            yield return new WaitUntil(() => !PollinationDance.isPlaying);
            Debug.Log("Dance complete.");
            if (CanPollinate(m_CurrentFlower))
            {
                Debug.Log("Pollinating.");
                if (m_CurrentFlower.ReceivePollen(BodyState.YieldPollen()))
                {
                    DidPollinate = true;
                    m_ParticleSystem.Stop();
                    SuccessfulPollination.Raise();
                }
            }
            else
            {
                Debug.Log("Covering self in pollen.", this);
                var pollen = m_CurrentFlower.GivePollen();
                BodyState.CoverSelf(pollen);
                m_ParticleMain.startColor = pollen.Color;
                m_ParticleSystem.Play();
            }
            m_GroundedActions.Enable();
            BeeLookOverride.enabled = true;
        }

        internal void FinishLanding(Flower flower)
        {
            m_CurrentFlower = flower;
        }

    }
}
