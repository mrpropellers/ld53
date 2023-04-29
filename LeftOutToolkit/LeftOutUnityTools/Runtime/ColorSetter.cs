using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LeftOut
{
    public class ColorSetter : MonoBehaviour
    {
        Color m_LastColorSet;

        public Color ColorToUse;
        [FormerlySerializedAs("Targets")]
        public List<SpriteRenderer> SpriteTargets;
        public List<MeshRenderer> MeshTargets;

        // Start is called before the first frame update
        void Start()
        {
            ApplyColor();
        }

        // Update is called once per frame
        void Update()
        {
            if (ColorToUse != m_LastColorSet)
            {
                ApplyColor();
            }
        }

        void OnValidate()
        {
            //ApplyColor();
        }

        void ApplyColor()
        {
            foreach (var spriteRenderer in SpriteTargets)
            {
                spriteRenderer.color = ColorToUse;
            }

            var mpb = new MaterialPropertyBlock();
            mpb.SetColor(GlobalConsts.ShaderProperty.MainColor, ColorToUse);
            foreach (var meshRenderer in MeshTargets)
            {
                meshRenderer.SetPropertyBlock(mpb);
            }

            m_LastColorSet = ColorToUse;
        }
    }
}
