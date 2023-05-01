using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace LeftOut.LudumDare
{
    public class GroundedCameraSwitcher : MonoBehaviour
    {
        float m_LastObservedYaw;
        float m_TimeLastMoved;

        [SerializeField]
        float StopTime = 4f;
        [SerializeField]
        CinemachineVirtualCamera StaticCamera;
        [SerializeField]
        CinemachineVirtualCamera MovingCamera;
        [SerializeField]
        Transform MovementTarget;

        bool HasMovedRecently => Time.time - m_TimeLastMoved < StopTime;

        void OnEnable()
        {
            m_TimeLastMoved = float.NegativeInfinity;
            StaticCamera.enabled = true;
            MovingCamera.enabled = false;
            m_LastObservedYaw = MovementTarget.transform.eulerAngles.y;
        }

        void OnDisable()
        {
            StaticCamera.enabled = false;
            MovingCamera.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            var newYaw = MovementTarget.transform.eulerAngles.y;
            if (Mathf.Approximately(m_LastObservedYaw, newYaw))
            {
                if (!HasMovedRecently)
                    ActivateAndDeactivate(StaticCamera, MovingCamera);
            }
            else
            {
                m_TimeLastMoved = Time.time;
                ActivateAndDeactivate(MovingCamera, StaticCamera);
            }

            m_LastObservedYaw = newYaw;
        }

        void ActivateAndDeactivate(CinemachineVirtualCamera activate, CinemachineVirtualCamera deactivate)
        {
            if (activate.enabled && !deactivate.enabled)
            {
                return;
            }
            activate.enabled = true;
            deactivate.enabled = false;
        }
    }
}
