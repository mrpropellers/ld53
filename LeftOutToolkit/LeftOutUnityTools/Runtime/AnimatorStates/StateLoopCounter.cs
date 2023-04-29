using UnityEngine;

namespace LeftOut.JamAids
{
    public class StateLoopCounter : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetInteger(GlobalConsts.AnimatorParameters.NumLoopsCurrentState, 0);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetInteger(GlobalConsts.AnimatorParameters.NumLoopsCurrentState, -1);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetInteger(GlobalConsts.AnimatorParameters.NumLoopsCurrentState,
                Mathf.FloorToInt(stateInfo.normalizedTime));
        }
    }
}
