using UnityEngine;

namespace LeftOut.GameplayManagement
{
    public enum ApplicationStateTypes
    {
        Unknown,
        SplashScreen,
        StartScreen,
        InGame,
        Exiting
    }

    [CreateAssetMenu(fileName = "ApplicationState", menuName = "Left Out/Application State", order = 0)]
    public class ApplicationState : StateMachine<ApplicationStateTypes>
    {
        protected override ApplicationStateTypes DefaultState => ApplicationStateTypes.Unknown;
    }
}
