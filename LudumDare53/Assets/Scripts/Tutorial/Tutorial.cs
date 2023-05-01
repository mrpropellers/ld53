using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.InputSystem;
using Void = UnityAtoms.Void;

namespace LeftOut.LudumDare.Tutorial
{
    public class Tutorial : MonoBehaviour
    {
        const string k_KeyboardName = "KeyboardMouse";
        const string k_GamepadName = "Gamepad";
        
        [System.Serializable]
        struct SpriteBindings
        {
            [SerializeField]
            TMPro.TMP_SpriteCharacter KeyboardMouse;
            [SerializeField]
            TMPro.TMP_SpriteCharacter Gamepad;

            internal TMPro.TMP_SpriteCharacter GetSprite(string controlScheme)
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
        class Prompt
        {
            internal bool HasResolved { get; private set; }

            [field: SerializeField]
            internal TMP_Text PromptText { get; private set; }
            [field: SerializeField]
            internal bool ShouldQueue { get; private set; }
            // Event that triggers our tutorial prompt
            [field: SerializeField]
            internal UnityAtoms.AtomEventBase Trigger { get; private set; }
            // Event that "defuses" the prompt (makes it no longer necessary)
            [field: SerializeField]
            internal UnityAtoms.AtomEventBase Defuse { get; private set; }
            [SerializeField]
            List<SpriteBindings> Sprites;

            public override string ToString() => PromptText?.name;

            internal void Reset()
            {
                PromptText.alpha = 0;
            }

            internal void MarkResolved()
            {
                HasResolved = true;
                Trigger.UnregisterAll();
                Defuse.UnregisterAll();
            }
        }

        string m_CurrentControlScheme;
        Prompt m_NextPrompt;
        bool m_IsPlayingPrompt;

        [SerializeField]
        List<Prompt> Prompts;
        [SerializeField]
        float GameStartDelay = 3f;
        [SerializeField]
        UnityAtoms.AtomEvent<PlayerInput> ControlSchemeChanged;
        [SerializeField]
        UnityAtoms.AtomEventBase GameStartEvent;

        void Start()
        {
            Debug.Log("Resetting all Tutorial prompts.");
            foreach (var prompt in Prompts)
            {
                prompt.Reset();
                prompt.Trigger.Register(() => HandlePromptTrigger(prompt));
                prompt.Defuse.Register(() => HandlePromptDefused(prompt));
            }

            StartCoroutine(RaiseAfter(GameStartEvent, GameStartDelay));
        }

        void OnEnable()
        {
            ControlSchemeChanged.Register(HandleControlSchemeChange);
        }

        void OnDisable()
        {
            Debug.Log("Unregistering callbacks");
            ControlSchemeChanged.Register(HandleControlSchemeChange);
            foreach (var prompt in Prompts.Where(prompt => !prompt.HasResolved))
            {
                prompt.MarkResolved();
            }
        }

        IEnumerator RaiseAfter(UnityAtoms.AtomEventBase atomEvent, float delay)
        {
            yield return new WaitForSeconds(delay);
            atomEvent.Raise();
        }

        void HandlePromptTrigger(Prompt prompt)
        {
            if (m_IsPlayingPrompt)
            {
                if (prompt.ShouldQueue)
                {
                    if (m_NextPrompt == prompt)
                    {
                        Debug.Log($"Doing nothing with {prompt} because it's already queued up.");
                        return;
                    }
                    if (m_NextPrompt != null)
                    {
                        Debug.Log($"Booting {m_NextPrompt} to make way for {prompt}");
                    }

                    m_NextPrompt = prompt;
                }
                else
                {
                    Debug.Log("Doing nothing, already busy and can't queue this prompt.");
                }
                return;
            }

            // If we get to here, no prompts playing right now, let's start one!
            StartCoroutine(PlayPrompt(prompt));
        }
        

        void HandlePromptDefused(Prompt prompt)
        {
            
        }

        IEnumerator PlayPrompt(Prompt prompt)
        {
            m_IsPlayingPrompt = true;
            Debug.Log(prompt.PromptText.text);
            yield return null;
            m_IsPlayingPrompt = false;
        }

        void HandleControlSchemeChange(PlayerInput playerInput)
        {
            Debug.Log(
                $"Control scheme changing from {m_CurrentControlScheme} to {playerInput.currentControlScheme}");
            m_CurrentControlScheme = playerInput.currentControlScheme;
        }
    }
}
