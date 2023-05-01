using UnityEngine;

namespace LeftOut.LudumDare.Tutorial
{
    public class TutorialState : MonoBehaviour 
    {
        [System.Serializable]
        struct PromptsTracker
        {
            [SerializeField]
            bool YawRotation;
            [SerializeField]
            bool TakeOff;
            [SerializeField]
            bool Throttle;
            [SerializeField]
            bool Land;
            [SerializeField]
            bool HarvestPollen;
            [SerializeField]
            bool DepositPollen;
        }

        PromptsTracker ButtonPrompts;

        void Start()
        {
            ButtonPrompts = new PromptsTracker();
        }
        
        
    }
}
