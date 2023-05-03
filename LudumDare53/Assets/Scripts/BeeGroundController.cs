using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace LeftOut.LudumDare
{
    [RequireComponent(typeof(BeeBodyState))]
    public class BeeGroundController : MonoBehaviour
    {
        public const string k_ActionMapName = "BeeLanded";
        const string k_ShaderBaseColor = "_Color";
        const string k_ShaderEmissionColor = "_EmissionColor";

        Flower m_PreviousFlower;
        Flower m_CurrentFlower;
        InputActionMap m_GroundedActions;
        ParticleSystemRenderer m_ParticleRenderer;
        
        [FormerlySerializedAs("m_ParticleSystem")]
        [SerializeField]
        ParticleSystem ParticleSystem;

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
            m_ParticleRenderer = ParticleSystem.GetComponent<ParticleSystemRenderer>();
            ParticleSystem.Stop();
        }

        bool CanPollinate(Flower flower)
        {
            return BodyState.HasPollen && BodyState.PollenSource != flower;
        }

        bool CanHarvestPollen(Flower flower)
        {
            return !BodyState.HasPollen && m_PreviousFlower != flower;
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
            
            if (!CanPollinate(m_CurrentFlower) && !CanHarvestPollen(m_CurrentFlower))
            {
                Debug.Log("Nothing to do on this flower.");
            }
            else
            {
                m_GroundedActions.Disable();
                BeeLookOverride.enabled = false;
                StartCoroutine(AnimatePollinate());
            }
        }

        IEnumerator AnimatePollinate()
        {
            PollinationDance.Play();
            yield return new WaitUntil(() => !PollinationDance.isPlaying);
            if (CanPollinate(m_CurrentFlower))
            {
                Debug.Log("Pollinating.");
                if (m_CurrentFlower.ReceivePollen(BodyState.YieldPollen()))
                {
                    DidPollinate = true;
                    ParticleSystem.Stop();
                    SuccessfulPollination.Raise();
                }
                else
                {
                    Debug.LogError("This should never happen?");
                }
            }
            else if(CanHarvestPollen(m_CurrentFlower))
            {
                Debug.Log("Covering self in pollen.", this);
                var pollen = m_CurrentFlower.GivePollen();
                BodyState.CoverSelf(pollen);
                var mbp = new MaterialPropertyBlock();
                mbp.SetColor(k_ShaderBaseColor, pollen.Color);
                mbp.SetColor(k_ShaderEmissionColor, pollen.Color);
                m_ParticleRenderer.SetPropertyBlock(mbp);
                ParticleSystem.Play();
            }
            else
            {
                Debug.LogError("We shouldn't have started the coroutine if there's nothing to do.");
            }

            m_PreviousFlower = m_CurrentFlower;
            m_GroundedActions.Enable();
            BeeLookOverride.enabled = true;
        }

        internal void FinishLanding(Flower flower)
        {
            m_CurrentFlower = flower;
        }

    }
}
