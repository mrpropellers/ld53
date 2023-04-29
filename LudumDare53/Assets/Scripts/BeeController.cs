using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeftOut.LudumDare
{
    [RequireComponent(typeof(Rigidbody))]
    public class BeeController : MonoBehaviour
    {
        Rigidbody m_Rigidbody;
        BeeSensorProcessor m_FlowerSensor;
        Vector2 m_MoveInput;
        bool m_IsFlying;

        [SerializeField]
        InputActionReference MoveAction;
        [SerializeField]
        InputActionReference ThrottleAction;

        // [SerializeField]
        // [Min(10f)]
        // float Acceleration = 40f;

        [SerializeField]
        float BaseSpeed = 5f;
        [SerializeField]
        float CruisingSpeed = 13.5f;
        [SerializeField]
        Vector2 MaximumTurnVelocity = new Vector2(30f, 30f);
        //[SerializeField]
        //AnimationCurve BeeAcceleration;


        // Start is called before the first frame update
        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_IsFlying = false;
            m_FlowerSensor = GetComponentInChildren<BeeSensorProcessor>();
        }

        void Update()
        {
            var rotation = MoveAction.action.ReadValue<Vector2>() * Time.deltaTime;
            rotation.Scale(MaximumTurnVelocity);
            transform.Rotate(rotation.y, rotation.x, 0);

            var speed = ThrottleAction.action.IsPressed() ? CruisingSpeed : BaseSpeed;
            var displacement = speed * Time.deltaTime * transform.forward;
            transform.position += displacement;

            if (m_FlowerSensor.DoesSenseFlower)
            {
                Debug.Log(m_FlowerSensor.ClosestFlower);
            }
        }

        public void OnThrottle()
        {
            Debug.Log("It happened!");
            // Debug.Log($"Throttle performed: {context.performed} - Toggling isFlying: {m_IsFlying}");
            // if (context.performed)
            // {
            m_IsFlying = !m_IsFlying;
            // }
        }
    }
}
