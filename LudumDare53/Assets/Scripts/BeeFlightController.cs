using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeftOut.LudumDare
{
    [RequireComponent(typeof(Rigidbody))]
    public class BeeFlightController : MonoBehaviour
    {
        Rigidbody m_Rigidbody;
        BeeFlowerSensor m_FlowerFlowerSensor;
        Vector3 m_RotationalTorque;
        float m_Speed;

        [SerializeField]
        InputActionReference MoveAction;
        [SerializeField]
        InputActionReference ThrottleAction;

        [SerializeField]
        float BaseSpeed = 5f;
        [SerializeField]
        float CruisingSpeed = 13.5f;
        [SerializeField]
        float MaxAngularVelocity = 4f;


        // Start is called before the first frame update
        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_FlowerFlowerSensor = GetComponentInChildren<BeeFlowerSensor>();
            
        }

        void Update()
        {
            var moveInput = MoveAction.action.ReadValue<Vector2>();
            m_RotationalTorque = new Vector3(moveInput.y, moveInput.x, 0);
            
            m_Speed = ThrottleAction.action.IsPressed() ? CruisingSpeed : BaseSpeed;
        }

        void FixedUpdate()
        {
            // TODO: Clamp pitch to keep player from flipping bee over
            // TODO: Damp velocity when no new torque is being applied
            m_Rigidbody.AddRelativeTorque(m_RotationalTorque);
            m_Rigidbody.maxAngularVelocity = MaxAngularVelocity;
            m_Rigidbody.velocity = m_Rigidbody.transform.forward * m_Speed;
        }
    }
}
