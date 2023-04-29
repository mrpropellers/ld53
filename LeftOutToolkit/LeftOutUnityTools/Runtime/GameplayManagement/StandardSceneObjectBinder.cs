using System;
using UnityEngine;

namespace LeftOut.GameplayManagement
{
    public class StandardSceneObjectBinder : MonoBehaviour, IBindToLevelStart, IDeactivateOnPause
    {
        [SerializeField]
        MonoBehaviour m_BindTarget;

        public Action OnLevelStart => ((IBindToLevelStart) m_BindTarget).OnLevelStart;
        public MonoBehaviour DeactivateThisOnPause => m_BindTarget;

        void OnValidate()
        {
            IsBindTargetValid();
        }

        bool IsBindTargetValid()
        {
            if (m_BindTarget == null)
            {
                return false;
            }
            if (!(m_BindTarget is IBindToLevelStart))
            {
                Debug.LogWarning(
                    $"Can't apply standard bindings to {m_BindTarget.name} " +
                    $"because it does not implement {nameof(IBindToLevelStart)}");
                return false;
            }

            return true;
        }

        void Awake()
        {
            if (!IsBindTargetValid())
            {
                return;
            }

            if (SceneStateBehaviour.Instance == null)
            {
                Debug.LogWarning($"{name} found no {typeof(SceneStateBehaviour)} - " +
                    $"won't bind to state transitions.");
                return;
            }

            SceneStateBehaviour.Instance.State.AutoBind(this);
        }
    }
}
