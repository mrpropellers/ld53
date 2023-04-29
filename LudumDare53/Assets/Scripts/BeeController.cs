using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeftOut.LudumDare
{
    [RequireComponent(typeof(Rigidbody))]
    public class BeeController : MonoBehaviour
    {
        Rigidbody m_Rigidbody;
        Vector2 m_MoveInput;

        [SerializeField]
        InputActionReference MoveAction;

        // [SerializeField]
        // [Min(10f)]
        // float Acceleration = 40f;

        [SerializeField]
        float MaximumVelocity = 13.5f;


        // Start is called before the first frame update
        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            var direction = MoveAction.action.ReadValue<Vector2>();
            Debug.Log($"Applying {direction} to self");
            m_MoveInput = MaximumVelocity * direction;
        }

        void FixedUpdate()
        {
            // rif (m_MoveInput.Equals(Vector2.zero))
            // r    return;
            // m_Rigidbody.velocity = m_MoveInput;
            m_Rigidbody.velocity = Vector3.forward;
        }
    }
}
