using System;
using LeftOut.Channels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Assert = UnityEngine.Assertions.Assert;

namespace LeftOut.GameplayManagement
{
    public class LevelTimer : SingletonBehaviour<LevelTimer>, IDeactivateOnPause
    {
        [SerializeField]
        float m_CompletionTime;

        [SerializeField]
        AnimationCurve m_DifficultyCurve;

        [field: SerializeField]
        public UnityEvent OnTimerComplete;

        [SerializeField]
        EventChannel m_StartChannel;

        [FormerlySerializedAs("m_ElapsedTimeChannel")]
        [SerializeField]
        FloatChannel m_TimeLeftChannel;


        public bool TimerHasFinished { get; private set; }
        public float ElapsedTime
        {
            get => (m_CompletionTime - m_TimeLeftChannel.Value);
            private set => m_TimeLeftChannel.Value = (m_CompletionTime - value);
        }

        public bool HasStarted { get; private set; }
        public bool IsRunning { get; private set; }
        public bool CanBeStarted => !HasStarted && !(TimerHasFinished || IsRunning);
        public float CompletionPercentage => Mathf.Clamp01(ElapsedTime / m_CompletionTime);
        public float CurrentDifficulty => Mathf.Clamp01(m_DifficultyCurve.Evaluate(CompletionPercentage));

        public MonoBehaviour DeactivateThisOnPause => this;

        protected void Start()
        {
            m_StartChannel.OnEvent.AddListener(HandleStartEvent);
            Reset();
        }

        public void Reset()
        {
            ElapsedTime = 0f;
            IsRunning = false;
            HasStarted = false;
            TimerHasFinished = false;
        }

        public void StartTimer()
        {
            Assert.IsFalse(IsRunning);
            Assert.IsFalse(HasStarted);
            HasStarted = true;
            IsRunning = true;
        }

        void Update()
        {
            if (IsRunning)
            {
                Assert.IsFalse(TimerHasFinished);
                ElapsedTime += Time.deltaTime;
                if (ElapsedTime >= m_CompletionTime)
                {
                    BroadcastTimeUp();
                }
            }
        }

        void BroadcastTimeUp()
        {
            IsRunning = false;
            TimerHasFinished = true;
            OnTimerComplete?.Invoke();
        }

        void HandleStartEvent()
        {
            if (!CanBeStarted)
            {
                Debug.LogError("Start event was raised after we already started.");
                return;
            }

            StartTimer();
        }
    }
}
