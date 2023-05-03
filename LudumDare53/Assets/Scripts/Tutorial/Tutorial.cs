using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityAtoms;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LeftOut.LudumDare.Tutorial
{
    public class Tutorial : MonoBehaviour, IAtomListener<InputAction.CallbackContext>
    {
        const string k_KeyboardName = "KeyboardMouse";
        const string k_GamepadName = "Gamepad";
        
        [System.Serializable]
        struct ControlSpecificText
        {
            [SerializeField]
            string KeyboardMouse;
            [SerializeField]
            string Gamepad;

            internal string GetForScheme(string controlScheme)
            {
                switch (controlScheme)
                {
                    case k_KeyboardName:
                        return KeyboardMouse;
                    case k_GamepadName:
                        return Gamepad;
                    default:
                        Debug.LogWarning($"No handling for {controlScheme} defined. Defaulting to GamePad");
                        return Gamepad;
                }
            }
        }

        [System.Serializable]
        class Prompt : IAtomListener
        {
            internal enum Status
            {
                Uninitialized,
                Latent,
                Active,
                Defused,
                Complete
            }
            
            const string k_ControlToken = "BUTTON";

            internal Action<Prompt> OnTutorialReady;
            internal Status CurrentStatus = Status.Uninitialized;
            internal float LastTriggered = float.NegativeInfinity;

            [SerializeField]
            string PromptTemplate;
            
            [field: SerializeField]
            [field: Min(0f)]
            internal float PreDelay { get; private set; }
            
            // Amount of time the prompt can stay in the queue before needing to be re-triggered
            // TODO: This should be replaced with a Condition check which checks whether conditions are still
            //       valid to fire this prompt once it reaches the front of the queue
            [field: SerializeField]
            [field: Min(0f)]
            internal float ExpirationTime { get; private set; }
            
            // TODO: Rework to allow for bespoke compound observations (or require other tutorials be completed first)
            // Event that triggers our tutorial prompt
            [field: SerializeField]
            internal AtomEventBase Trigger { get; private set; }
            
            // TODO: Merge this field into an ExitCondition box that can be either action performed OR delay
            [field: SerializeField]
            internal string ActionName { get; private set; }
            [field: SerializeField]
            internal float DisplayTime { get; private set; }
            [field: SerializeField]
            internal bool MustPerform { get; private set; }
            
            [SerializeField]
            List<ControlSpecificText> Substitutions;

            internal bool IsExpired => Time.time - LastTriggered > ExpirationTime;
            
            static bool TryReplaceOne(string text, string token, string replacement, out string result)
            {
                var pos = text.IndexOf(token);
                if (pos < 0)
                {
                    result = text;
                    return false;
                }

                result = text[..pos] + replacement + text[(pos + token.Length)..];
                return true;
            }

            public string GetPromptText(string controlScheme)
            {
                var prompt = PromptTemplate;
                foreach (var sub in Substitutions)
                {
                    var control = sub.GetForScheme(controlScheme);
                    if (!TryReplaceOne(prompt, k_ControlToken, control, out prompt))
                    {
                        Debug.LogError(
                            $"Failed to get all {Substitutions.Count} {nameof(Substitutions)} into {PromptTemplate}");
                    }
                }

                return prompt;
            }
            public override string ToString() => PromptTemplate;
            public void OnEventRaised()
            {
                switch (CurrentStatus)
                {
                    case Status.Uninitialized:
                        Debug.LogError("Prompt not initialized - can't do anything.");
                        return;
                    case Status.Latent:
                        if (OnTutorialReady.Target == null)
                        {
                            Debug.LogError("No callback set.");
                            return;
                        }
                        OnTutorialReady.Invoke(this);
                        break;
                    case Status.Defused:
                    case Status.Complete:
                        Debug.LogWarning("Prompt was triggered more than once, unsubscribing to callback.");
                        Trigger.UnregisterListener(this);
                        break;
                    case Status.Active:
                        Debug.LogWarning("Prompt triggered again while still displaying - doing nothing.");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        string m_CurrentControlScheme;
        Queue<Prompt> m_PromptsWaiting;
        Prompt m_ActivePrompt;
        bool IsPlayingPrompt => m_ActivePrompt != null;

        [SerializeField]
        PlayerInput PlayerInput;
        [SerializeField]
        Image PromptBackground;
        [SerializeField]
        TextMeshProUGUI PromptBox;
        [SerializeField]
        List<Prompt> Prompts;
        [SerializeField]
        float TextFadeTime = 2f;
        [SerializeField]
        float PromptTimeout = 10f;
        [SerializeField]
        UnityAtoms.AtomEvent<PlayerInput> ControlSchemeChanged;
        [SerializeField]
        AtomEvent<InputAction.CallbackContext> PlayerActionTaken;
        [SerializeField]
        UnityAtoms.AtomEventBase GameStartEvent;
        
        Prompt NextPrompt => m_PromptsWaiting.TryPeek(out var prompt) ? prompt : null;

        void Start()
        {
            Debug.Log("Resetting all Tutorial prompts.");
            m_CurrentControlScheme = PlayerInput.currentControlScheme;
            Debug.Log($"Current scheme: {m_CurrentControlScheme}");
            m_ActivePrompt = null;
            foreach (var prompt in Prompts)
            {
                prompt.OnTutorialReady = HandlePromptTrigger;
                prompt.Trigger.RegisterListener(prompt);
                prompt.CurrentStatus = Prompt.Status.Latent;
            }

            m_PromptsWaiting = new Queue<Prompt>();
        }

        void OnEnable()
        {
            PlayerInput.onControlsChanged += HandleControlsChange;
            PlayerActionTaken.RegisterListener(this);
        }

        void OnDisable()
        {
            Debug.Log("Unregistering callbacks");
            PlayerInput.onControlsChanged -= HandleControlsChange;
            PlayerActionTaken?.UnregisterListener(this);
            foreach (var prompt in Prompts)
            {
                prompt.Trigger.UnregisterListener(prompt);
            }
        }

        public void HandleControlsChange(PlayerInput playerInput)
        {
            Debug.Log(
                $"Control scheme changing from {m_CurrentControlScheme} to {playerInput.currentControlScheme}");
            m_CurrentControlScheme = playerInput.currentControlScheme;
        }

        public void OnEventRaised(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            if (IsPlayingPrompt && m_ActivePrompt.ActionName.Equals(context.action.name))
            {
                Debug.Log($"Tutorial observed: {context.action.name} {context.phase} -- processing.");
                if (m_ActivePrompt.CurrentStatus == Prompt.Status.Defused)
                {
                    Debug.Log("Prompt already defused, just waiting for completion.");
                    return;
                }
                if (m_ActivePrompt.CurrentStatus != Prompt.Status.Active)
                {
                    Debug.LogError("Active prompt not marked as such");
                    return;
                }
                Debug.Log($"Defusing {m_ActivePrompt}");
                Defuse(m_ActivePrompt);
            }
        }

        void Defuse(Prompt prompt)
        {
            if (prompt.CurrentStatus != Prompt.Status.Active)
            {
                Debug.LogError($"Can't defuse {prompt} which isn't currently active");
                return;
            }
            prompt.Trigger.UnregisterListener(prompt);
            prompt.CurrentStatus = Prompt.Status.Defused;
        }

        void HandlePromptTrigger(Prompt prompt)
        {
            prompt.LastTriggered = Time.time;
            if (IsPlayingPrompt && !m_PromptsWaiting.Contains(prompt))
            {
                Debug.Log($"Adding {prompt} to queue.");
                m_PromptsWaiting.Enqueue(prompt);
                return;
            }

            // If we get to here, no prompts playing right now, let's start one!
            m_PromptsWaiting.Enqueue(prompt);
            StartCoroutine(ProcessPromptQueue());
        }

        void SetPromptAlpha(float val)
        {
            PromptBox.alpha = val;
            var c = PromptBackground.color;
            c.a = val * 0.9f;
            PromptBackground.color = c;
        }
        
        IEnumerator ProcessPromptQueue()
        {
            while (m_PromptsWaiting.Any())
            {
                // TODO: This logic is messy could definitely be cleaned up with a little time/thought
                Prompt prompt;
                m_ActivePrompt = null;
                while (m_PromptsWaiting.TryDequeue(out prompt))
                {
                    if (prompt.IsExpired)
                    {
                        Debug.Log($"{prompt} expired in queue, discarding");
                        continue;
                    }
                    // If it's not expired, keep it and leave this loop
                    m_ActivePrompt = prompt;
                    break;
                }
                
                // Check whether we found a prompt that can be displayed
                if (!IsPlayingPrompt)
                    break;
                prompt.CurrentStatus = Prompt.Status.Active;
                var promptText = prompt.GetPromptText(m_CurrentControlScheme);
                Debug.Log(promptText);
                PromptBox.text = promptText;
                SetPromptAlpha(0f);
                yield return new WaitForSeconds(prompt.PreDelay);
                // Check whether prompt was defused during the predelay period
                if (prompt.CurrentStatus == Prompt.Status.Active)
                {
                    Debug.Log("Fade in");
                    DOVirtual.Float(0f, 1f, TextFadeTime, SetPromptAlpha);
                    yield return new WaitForSeconds(TextFadeTime);
                }

                // If no display time specified, we wait for the Action to happen
                if (Mathf.Approximately(prompt.DisplayTime, 0f))
                {
                    var startTime = Time.time;
                    while (Time.time - startTime < PromptTimeout && prompt.CurrentStatus == Prompt.Status.Active)
                        yield return null;
                    if (prompt.CurrentStatus == Prompt.Status.Active)
                    {
                        if (prompt.MustPerform)
                        {
                            Debug.LogWarning($"{prompt} timed out and must be performed. Recycling");
                            prompt.CurrentStatus = Prompt.Status.Latent;
                        }
                        else
                        {
                            Debug.LogWarning($"{prompt} timed out. Defusing...");
                            Defuse(prompt);
                        }
                    }
                }
                // Otherwise, we defuse when the displaytime has elapsed
                else
                {
                    yield return new WaitForSeconds(prompt.DisplayTime);
                    Defuse(prompt);
                }
                Debug.Log("Fade out.");
                DOVirtual.Float(1f, 0f, TextFadeTime, SetPromptAlpha);
                yield return new WaitForSeconds(TextFadeTime);
                if (prompt.CurrentStatus != Prompt.Status.Defused && prompt.CurrentStatus != Prompt.Status.Latent)
                {
                    Debug.LogWarning($"Something changed status unexpectedly to {prompt.CurrentStatus}: {prompt}");
                }
                prompt.CurrentStatus = Prompt.Status.Complete;
            }
        }

    }
}
