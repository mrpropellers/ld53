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
        Flower m_CurrentFlower;
        InputActionMap m_FlyingActions;
        InputActionMap m_GroundedActions;
        
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
        GroundedCameraSwitcher GroundedCamera;
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


        void Start()
        {
            if (StartingState == ControlState.Transition)
            {
                // We forgot to set the initial state in the inspector...
                Debug.LogError($"Bee starting in Transition state (NOT ALLOWED!)");
            }
            m_CurrentState = StartingState;
            m_FlightController = GetComponent<BeeFlightController>();
            m_GroundController = GetComponent<BeeGroundController>();
            m_PlayerInput = GetComponent<PlayerInput>();
            FlowerSensor = GetComponentInChildren<BeeFlowerSensor>();
            m_RootRigidBody = BeeRoot.GetComponent<Rigidbody>();
            m_FlyingActions = m_PlayerInput.actions.FindActionMap(FlyingActionMapName);
            m_GroundedActions = m_PlayerInput.actions.FindActionMap(GroundActionMapName);
            if (StartingState == ControlState.Grounded)
            {
                m_CurrentFlower = FlowerSensor.DetectCollidingFlower();
                if (m_CurrentFlower == null)
                {
                    Debug.LogError($"Can't start {m_CurrentState} because no Flower to sit on. " +
                        $"Switching to {ControlState.Flying}");
                    StartingState = ControlState.Flying;
                }
            }
            EnterTransition(StartingState);
            ExitTransition(StartingState);
        }

        public void OnTakeoff(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            switch (m_CurrentState)
            {
                case ControlState.Transition:
                    Debug.Log("Already in transition, doing nothing");
                    return;
                case ControlState.Flying:
                    Debug.LogError(
                        "Something is wrong with the input system - it invoked Takeoff while already flying...");
                    break;
                case ControlState.Grounded:
                    EnterTransition(ControlState.Flying);
                    StartCoroutine(TakeOff());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(m_CurrentState.ToString());
            }
        }
        
        public void OnLand(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }

            switch (m_CurrentState)
            {
                case ControlState.Transition:
                    Debug.Log("Already in transition, doing nothing.");
                    return;
                case ControlState.Grounded:
                    Debug.LogError(
                        "Something is wrong with the input system - it invoked Land while already grounded...");
                    break;
                case ControlState.Flying:
                    if (CanLand)
                    {
                        EnterTransition(ControlState.Grounded);
                        StartCoroutine(Land());
                    }
                    else
                    {
                        Debug.Log("Can't land. Doing nothing.");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(m_CurrentState.ToString());
            }
        }

        void EnterTransition(ControlState newState)
        {
            if (m_CurrentState == ControlState.Transition)
            {
                Debug.LogError("Can't enter transition when already in transition!");
            }
            switch (newState)
            {
                case ControlState.Grounded:
                    VisualRigidBody.isKinematic = true;
                    m_RootRigidBody.isKinematic = true;
                    m_FlightController.enabled = false;
                    break;
                case ControlState.Transition: 
                    Debug.LogError($"This method will set state to transition; don't set it yourself!");
                    break;
                default:
                    break;
            }

            m_CurrentState = ControlState.Transition;
        }

        void ExitTransition(ControlState newState)
        {
            if (m_CurrentState != ControlState.Transition)
            {
                Debug.LogError("Can't complete transition correctly if not in transition state!");
            }
            m_CurrentState = newState;
            switch (m_CurrentState)
            {
                case ControlState.Flying:
                    ResetRigidbody(m_RootRigidBody);
                    ResetRigidbody(VisualRigidBody);
                    m_PlayerInput.currentActionMap = m_FlyingActions;
                    m_FlightController.enabled = true;
                    FlyingCamera.enabled = true;
                    GroundedCamera.enabled = false;
                    break;
                case ControlState.Grounded:
                    m_PlayerInput.currentActionMap = m_GroundedActions;
                    FlyingCamera.enabled = false;
                    GroundedCamera.enabled = true;
                    m_GroundController.FinishLanding(m_CurrentFlower);
                    break;
                case ControlState.Transition: 
                    Debug.LogError($"Can't exit transition INTO a transition!");
                    break;
                default:
                    break;
            }
        }
        
        static void ResetRigidbody(Rigidbody body)
        {
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            body.isKinematic = false;
        }

        IEnumerator TakeOff()
        {
            yield return null;
            ExitTransition(ControlState.Flying);
        }

        IEnumerator Land()
        {
            void DoLandTween(Transform tf, Transform target, float time)
            {
                tf.DOMove(target.position, time).SetEase(LandingEase);
                tf.DORotate(target.rotation.eulerAngles, time).SetEase(LandingEase);
            }
            m_CurrentFlower = FlowerSensor.ClosestFlower;
            var target = m_CurrentFlower.LandingPointCenter;
            var landingTime = (VisualRigidBody.position - target.position).magnitude / LandingSpeed;
            DoLandTween(BeeRoot.transform, target, landingTime);
            DoLandTween(VisualRigidBody.transform, target, landingTime);
            yield return new WaitForSeconds(landingTime);
            ExitTransition(ControlState.Grounded);
        }

    }
}
