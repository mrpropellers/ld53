using UnityEngine;

namespace LeftOut.GameplayManagement
{
    public interface IDeactivateOnPause
    {
        public MonoBehaviour DeactivateThisOnPause { get; }
    }

    public interface IBindToLevelStart
    {
        public System.Action OnLevelStart { get; }
    }
}
