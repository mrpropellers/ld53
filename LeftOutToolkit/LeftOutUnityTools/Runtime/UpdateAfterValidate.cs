using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace LeftOut
{
    [ExecuteAlways]
    public class UpdateAfterValidate : MonoBehaviour
    {
#if UNITY_EDITOR
        List<IUpdateAfterValidate> m_UpdateNeeders;

        void Update()
        {
            foreach (var component in GetComponents<IUpdateAfterValidate>())
            {
                if (component.NeedsUpdate)
                {
                    component.DoOnValidateUpdate();
                }
            }
        }
#endif
    }
}
