using System;
using System.Collections;
using Cinemachine;
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
        
        BeeFlightController m_FlightController;
        BeeGroundController m_GroundController;
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
            m_FlightController.enabled = true;
            yield return null;
            m_CurrentState = ControlState.Flying;
            SetUpForCurrentState();
        }

        IEnumerator Land()
        {
            var flower = FlowerSensor.ClosestFlower;
            var target = flower.LandingPointCenter;
            BeeRoot.SetPositionAndRotation(target.position, target.rotation);
            m_FlightController.enabled = false;
            yield return null;
            m_CurrentState = ControlState.Grounded;
            m_GroundController.FinishLanding(flower);
            SetUpForCurrentState();
        }

    }
}
