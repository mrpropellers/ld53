using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace LeftOut.LudumDare
{
    public class FocalLengthAdjuster : MonoBehaviour
    {
        [SerializeField]
        Camera MainCamera;

        [SerializeField]
        Rigidbody BeeRoot;

        // [SerializeField]
        // float AdjustmentSpeed = 0.5f;

        DepthOfField m_GlobalDepthOfField;

        void Start()
        {
            m_GlobalDepthOfField = (DepthOfField)GetComponent<Volume>().profile.components.Find(c => c is DepthOfField);
        }

        void Update()
        {
            var distance = (MainCamera.transform.position - BeeRoot.position).magnitude;
            // var delta = m_GlobalDepthOfField.focusDistance.value - distance;
            // if (Mathf.Abs(delta) > AdjustmentSpeed * Time.deltaTime)
            // {
            //     
            // }

            m_GlobalDepthOfField.focusDistance = new MinFloatParameter(distance, 0.1f);
        }
    }
}
