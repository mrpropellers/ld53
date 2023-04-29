using UnityEngine;

namespace LeftOut.JamAids
{
    public class TransformForwardProvider : MonoBehaviour, IForwardProvider
    {
        public Vector3 Forward => transform.forward;
    }
}
