using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Animator))]
public class AnimatorSpeedSetter : MonoBehaviour
{
    Animator m_Animator;

    [SerializeField]
    [Range(0f, 1f)]
    float m_AnimatorSpeed;

    // Start is called before the first frame update
    void OnEnable()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Animator.speed = m_AnimatorSpeed;
    }
}
