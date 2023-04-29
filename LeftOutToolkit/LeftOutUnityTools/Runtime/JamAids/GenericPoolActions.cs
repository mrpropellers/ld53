using UnityEngine;

namespace LeftOut.JamAids
{
    public static class GenericPoolActions
    {
        public static void ActivateAndEnable<T>(T monoBehaviour) where T : MonoBehaviour
        {
            monoBehaviour.gameObject.SetActive(true);
            monoBehaviour.enabled = true;
        }

        public static void DeactivateAndDisable<T>(T monoBehaviour) where T : MonoBehaviour
        {
            monoBehaviour.enabled = false;
            monoBehaviour.gameObject.SetActive(false);
        }
    }
}
