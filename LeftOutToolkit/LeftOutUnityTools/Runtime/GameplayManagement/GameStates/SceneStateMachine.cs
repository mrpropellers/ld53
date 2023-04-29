using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace LeftOut.GameplayManagement
{
    public enum SceneState
    {
        Unknown,
        Initialized,
        PreStart,
        Active,
        Paused,
        PreComplete,
        Complete
    }

    [CreateAssetMenu(fileName = "SceneState", menuName = "Left Out/Scene State", order = 0)]
    public class SceneStateMachine : StateMachine<SceneState>
    {
        enum LevelEndType
        {
            Unknown,
            Exited,
            Success,
            Failed
        }

        // TODO: Evaluate which I like better: All events handled as SceneState transition events,
        //       or with a bunch of specific-purpose EventChannel objects
        //       Might be good to look into more tightly coupling EventChannel subtypes to specific state
        //       transitions in the global state object
        [SerializeField]
        EventChannel m_StartLevelChannel;

        [SerializeField]
        bool m_DEBUG_StartLevel;
        [SerializeField]
        bool m_DEBUG_Reset;

        [SerializeField]
        SceneState m_DEBUG_CurrentState;

        protected override SceneState DefaultState => SceneState.Unknown;

        void OnValidate()
        {
            if (m_DEBUG_StartLevel)
            {
                Debug.Log("Transitioning to Level Start...");
                TryTransitionTo(SceneState.Active);
                m_DEBUG_StartLevel = false;
            }

            if (m_DEBUG_Reset)
            {
                Reset();
                m_DEBUG_Reset = false;
            }
            m_DEBUG_CurrentState = Current;
        }

        public override void Initialize()
        {
            Reset();
            if (m_StartLevelChannel != null)
            {
                m_StartLevelChannel.OnEvent.AddListener(() => TryTransitionTo(SceneState.Active));
            }

            Current = SceneState.Initialized;
        }

        public void AutoBind(UnityEngine.Object obj)
        {
            if (obj is IDeactivateOnPause deactivateOnPause)
            {
                Debug.Log($"Binding {obj.name} to pause/unpause.");
                BindSpecificTransition(SceneState.Active, SceneState.Paused,
                    () => deactivateOnPause.DeactivateThisOnPause.enabled = false);
                BindSpecificTransition(SceneState.Paused, SceneState.Active,
                    () => deactivateOnPause.DeactivateThisOnPause.enabled = true);
            }

            if (obj is IBindToLevelStart bindToLevelStart)
            {
                Debug.Log($"Binding {obj.name} to level start.");
                BindSpecificTransition(SceneState.Initialized, SceneState.Active,
                    bindToLevelStart.OnLevelStart);
            }
        }
    }
}
