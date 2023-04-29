using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LeftOut.GameplayManagement
{
    public abstract class StateMachine<T> : ScriptableObject
    {
        struct Transition
        {
            internal T From { get; private set; }
            internal T To { get; private set; }

            internal Transition(T from, T to)
            {
                From = from;
                To = to;
            }

            public override string ToString() => $"{From} => {To}";
        }


        protected abstract T DefaultState { get; }

        T m_State;

        public T Current
        {
            get => m_State;
            protected set
            {
                if (m_State.Equals(value))
                    return;
                Debug.Log($"Transitioning from {m_State} to {value}");
                var oldState = m_State;
                m_State = value;
                InvokeTransitionBindings(new Transition(oldState, m_State));
                OnStateTransition?.Invoke(oldState, m_State);
            }
        }

        Dictionary<Transition, List<Action>> m_TransitionBindings =
            new Dictionary<Transition, List<Action>>();

        public virtual bool TryTransitionTo(T to)
        {
            Current = to;
            return true;
        }

        void Awake()
        {
            Debug.Log($"Setting {name} to {DefaultState}");
            Current = DefaultState;
        }

        protected void Reset()
        {
            OnReset?.Invoke();
            Current = DefaultState;
            // TODO? Clear all bindings here
        }

        public virtual void Initialize()
        {
            Reset();
        }

        [field: SerializeField]
        public UnityEvent
            OnReset { get; private set; }
        [field: SerializeField]
        public UnityEvent<T, T> OnStateTransition { get; private set; }

        // TODO: Probably want to add Binds for just (T from) and (T to)
        public void BindSpecificTransition(T from, T to, Action callback)
        {
            var transition = new Transition(from, to);
            if (!m_TransitionBindings.TryGetValue(transition, out var bindings))
            {
                Debug.Log($"Adding callbacks list for {transition}");
                bindings = new List<Action>();
                m_TransitionBindings[transition] = bindings;
            }
            bindings.Add(callback);
        }

        void InvokeTransitionBindings(Transition transition)
        {
            if (m_TransitionBindings.TryGetValue(transition, out var bindings))
            {
                foreach (var callback in bindings)
                {
                    // TODO: Need to investigate how to detect whether Action target has been GC'd --
                    //      Also need to confirm the Action reference isn't preventing GC of destroyed objects
                    callback?.Invoke();
                }
            }
            else
            {
                Debug.Log($"No bindings for {transition}");
            }
        }
    }
}
