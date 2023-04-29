using System;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace LeftOut.Atoms
{
    public class IntFractionCalculator : MonoBehaviour, IAtomListener<int>
    {
        [SerializeField]
        IntReference m_Numerator;

        [SerializeField]
        IntReference m_Denominator;

        [SerializeField]
        FloatReference m_Ratio;

        void Awake()
        {
            GetEventIfExists(m_Numerator)?.RegisterListener(this);
            GetEventIfExists(m_Denominator)?.RegisterListener(this);
        }

        void OnDestroy()
        {
            GetEventIfExists(m_Numerator)?.UnregisterListener(this);
            GetEventIfExists(m_Denominator)?.UnregisterListener(this);
        }

        IntEvent GetEventIfExists(IntReference atom) =>
            atom.Usage switch
            {
                AtomReferenceUsage.VALUE => null,
                AtomReferenceUsage.CONSTANT => null,
                _ => atom.GetEvent<IntEvent>()
            };

        public void OnEventRaised(int _)
        {
            m_Ratio.Value = (float)m_Numerator.Value / m_Denominator.Value;
        }
    }
}
