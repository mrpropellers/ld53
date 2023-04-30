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
        Vector2 m_MoveInput;

        [SerializeField]
        InputActionReference MoveAction;
        [SerializeField]
        InputActionReference ThrottleAction;

        [SerializeField]
        float BaseSpeed = 5f;
        [SerializeField]
        float CruisingSpeed = 13.5f;
        [SerializeField]
        Vector2 MaximumTurnVelocity = new Vector2(30f, 30f);


        // Start is called before the first frame update
        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_FlowerFlowerSensor = GetComponentInChildren<BeeFlowerSensor>();
            
        }

        void Update()
        {
            var rotation = MoveAction.action.ReadValue<Vector2>() * Time.deltaTime;
            rotation.Scale(MaximumTurnVelocity);
            transform.Rotate(rotation.y, rotation.x, 0);

            var speed = ThrottleAction.action.IsPressed() ? CruisingSpeed : BaseSpeed;
            var displacement = speed * Time.deltaTime * transform.forward;
            transform.position += displacement;
        }
    }
}
