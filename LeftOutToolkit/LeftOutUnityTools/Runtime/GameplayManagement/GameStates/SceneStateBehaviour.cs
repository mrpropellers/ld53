using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LeftOut.GameplayManagement
{
    // TODO? Maybe this shouldn't be a Singleton
    public class SceneStateBehaviour : SingletonBehaviour<SceneStateBehaviour>
    {

        [SerializeField]
        SceneStateMachine m_State;

        public SceneStateMachine State
        {
            get
            {
                if (m_State == null)
                {
                    Debug.LogWarning($"No {nameof(SceneStateMachine)} set - creating a blank one.");
                    m_State = ScriptableObject.CreateInstance<SceneStateMachine>();
                }
                return m_State;
            }
        }

        void Start()
        {
            m_State.Initialize();
        }
    }
}
