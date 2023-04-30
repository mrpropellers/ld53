
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LeftOut.LudumDare
{
    public class BeeGroundController : MonoBehaviour
    {
        [SerializeField]
        Transform BeeRoot;

        [SerializeField]
        InputActionReference GroundMoveAction;

        [SerializeField]
        float YawVelocity = 50f;
        
        void Update()
        {
            var rotation = GroundMoveAction.action.ReadValue<Vector2>().x;
            rotation *= Time.deltaTime * YawVelocity;
            BeeRoot.transform.Rotate(0, rotation, 0);
        }
    }
}
