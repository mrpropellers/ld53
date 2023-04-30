using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeftOut.LudumDare
{
    public class BeeLandingHandler : MonoBehaviour
    {
        enum ControlState
        {
            Transition,
            Grounded,
            Flying
        }

        ControlState m_CurrentState;
        PlayerInput m_PlayerInput;
        Rigidbody m_RootRigidBody;
        
        BeeFlightController m_FlightController;
        BeeGroundController m_GroundController;
        [SerializeField]
        Ease LandingEase = Ease.OutSine;
        [SerializeField]
        float LandingSpeed;
        [SerializeField]
        Rigidbody VisualRigidBody;
        [SerializeField]
        string FlyingActionMapName;
        [SerializeField]
        string GroundActionMapName;
        [SerializeField]
        CinemachineVirtualCamera FlyingCamera;
        [SerializeField]
        CinemachineVirtualCamera GroundedCamera;
        [SerializeField]
        public BeeFlowerSensor FlowerSensor;
        [SerializeField]
        Transform BeeRoot;
        [SerializeField]
        ControlState StartingState;

        public bool CanLand
        {
            get
            {
                if (FlowerSensor == null)
                {
                    Debug.LogError("Can't check landing, flower sensor is null");
                    return false;
                }
                    
                return FlowerSensor.DoesSenseFlower;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            m_CurrentState = StartingState;
            m_FlightController = GetComponent<BeeFlightController>();
            m_GroundController = GetComponent<BeeGroundController>();
            m_PlayerInput = GetComponent<PlayerInput>();
            FlowerSensor = GetComponentInChildren<BeeFlowerSensor>();
            m_RootRigidBody = BeeRoot.GetComponent<Rigidbody>();
            SetUpForCurrentState();
        }

        public void OnLandTakeoff(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            switch (m_CurrentState)
            {
                // TODO: Disable controls entirely during Transition
                case ControlState.Transition:
                    Debug.Log("Changing states -- can't do anything");
                    return;
                case ControlState.Grounded:
                    m_CurrentState = ControlState.Transition;
                    StartCoroutine(TakeOff());
                    break;
                case ControlState.Flying:
                    if (CanLand)
                    {
                        m_CurrentState = ControlState.Transition;
                        StartCoroutine(Land());
                    }
                    else
                    {
                        Debug.Log("No flowers to land on.");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void SetUpForCurrentState()
        {
            m_PlayerInput.currentActionMap = m_CurrentState switch
            {
                ControlState.Flying => m_PlayerInput.actions.FindActionMap(FlyingActionMapName),
                ControlState.Grounded => m_PlayerInput.actions.FindActionMap(GroundActionMapName),
                _ => m_PlayerInput.currentActionMap
            };
            switch (m_CurrentState)
            {
                case ControlState.Flying:
                    FlyingCamera.enabled = true;
                    GroundedCamera.enabled = false;
                    break;
                case ControlState.Grounded:
                    FlyingCamera.enabled = false;
                    GroundedCamera.enabled = true;
                    break;
                default:
                    break;
            }
        }

        IEnumerator TakeOff()
        {
            void ResetRigidbody(Rigidbody body)
            {
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
            m_FlightController.enabled = true;
            m_CurrentState = ControlState.Flying;
            ResetRigidbody(m_RootRigidBody);
            ResetRigidbody(VisualRigidBody);
            SetUpForCurrentState();
            yield return null;
            m_RootRigidBody.isKinematic = false;
            VisualRigidBody.isKinematic = false;
        }

        IEnumerator Land()
        {
            void DoLandTween(Transform tf, Transform target, float time)
            {
                tf.DOMove(target.position, time).SetEase(LandingEase);
                tf.DORotate(target.rotation.eulerAngles, time).SetEase(LandingEase);
            }
            var flower = FlowerSensor.ClosestFlower;
            var target = flower.LandingPointCenter;
            VisualRigidBody.isKinematic = true;
            m_RootRigidBody.isKinematic = true;
            var landingTime = (VisualRigidBody.position - target.position).magnitude / LandingSpeed;
            m_FlightController.enabled = false;
            DoLandTween(BeeRoot.transform, target, landingTime);
            DoLandTween(VisualRigidBody.transform, target, landingTime);
            yield return new WaitForSeconds(landingTime);
            m_CurrentState = ControlState.Grounded;
            m_GroundController.FinishLanding(flower);
            SetUpForCurrentState();
        }

    }
}
