using DG.Tweening;
using UnityEngine;

namespace LeftOut
{
    public class TurnAroundState : StateMachineBehaviour
    {
        Collider2D[] m_AllColliders;
        bool[] m_OriginalColliderStates;
        Quaternion m_LastRotation;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // m_AllColliders = animator.gameObject.GetComponentsInChildren<Collider2D>();
            // m_OriginalColliderStates = new bool[m_AllColliders.Length];
            // for (var i = 0; i < m_AllColliders.Length; ++i)
            // {
            //     m_OriginalColliderStates[i] = m_AllColliders[i].enabled;
            //     m_AllColliders[i].enabled = false;
            // }

            animator.transform.DOBlendableRotateBy(Vector3.up * 180f, stateInfo.length).SetRelative().SetEase(Ease.InOutQuad);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // for (var i = 0; i < m_AllColliders.Length; ++i)
            // {
            //     m_AllColliders[i].enabled = m_OriginalColliderStates[i];
            // }

            animator.transform.DOComplete();
        }
    }
}
