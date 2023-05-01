using UnityEngine;
using UnityEngine.InputSystem;

namespace LeftOut.LudumDare
{
    [RequireComponent(typeof(BeeBodyState))]
    public class BeeGroundController : MonoBehaviour
    {
        Flower m_CurrentFlower;

        [SerializeField]
        BeeBodyState BodyState;
        
        [SerializeField]
        Transform BeeRoot;

        [SerializeField]
        InputActionReference GroundMoveAction;

        [SerializeField]
        float YawVelocity = 50f;

        public bool DidPollinate { get; set; } = false;

        bool CanPollinate(Flower flower)
        {
            return BodyState.HasPollen && BodyState.PollenSource != flower;
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
            
            if (CanPollinate(m_CurrentFlower))
            {
                Debug.Log("Pollinating.");
                if (m_CurrentFlower.ReceivePollen(BodyState.YieldPollen()))
                {
                    DidPollinate = true;
                }
            }
            else if (!BodyState.HasPollen)
            {
                Debug.Log("Covering self in pollen.", this);
                BodyState.CoverSelf(m_CurrentFlower.GivePollen());
            }
        }
        
        internal void FinishLanding(Flower flower)
        {
            m_CurrentFlower = flower;
        }

    }
}
