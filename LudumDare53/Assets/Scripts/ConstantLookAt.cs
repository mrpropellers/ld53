using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeftOut.LudumDare
{
    public class ConstantLookAt : MonoBehaviour
    {
        [SerializeField]
        Transform Target;

        [SerializeField]
        Transform Center;

        [SerializeField]
        float UpOffset = 0.25f;
        
        void Update()
        {
            var tf = transform;
            var position = tf.position;
            var toTarget = (Target.position - position).normalized;
            var up = (Center.position + Vector3.up * UpOffset) - position;
            var newRotation = transform.rotation;
            newRotation.SetLookRotation(toTarget, up);
            transform.rotation = newRotation;
        }
    }
}
