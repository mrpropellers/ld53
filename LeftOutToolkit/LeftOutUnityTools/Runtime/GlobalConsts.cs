using UnityEngine;

namespace LeftOut.GlobalConsts
{
    public static class ShaderProperty
    {
        // TODO: Make this renderer agnostic with pre-processor defines
        public const string MainColor = "_BaseColor";
        public static int MainColorId = Shader.PropertyToID(MainColor);
        public const string EmissiveColor = "_EmissionColor";
        public static int EmissiveColorId = Shader.PropertyToID(EmissiveColor);
    }

    public static class Tags
    {
        public const string Untagged = "Untagged";
        public const string Respawn = "Respawn";
        public const string Finish = "Finish";
        public const string EditorOnly = "EditorOnly";
        public const string MainCamera = "MainCamera";
        public const string Player = "Player";
        public const string GameController = "GameController";
        public const string FrontAim = "FrontAim";
        public const string RearAim = "RearAim";
    }

    public static class AnimatorParameters
    {
        public static int NumLoopsCurrentState = Animator.StringToHash("NumLoopsCurrentState");
        public static int HitboxActive = Animator.StringToHash("HurtboxActive");
        public static int StartHurtboxWindup = Animator.StringToHash("WindUpHurtbox");
    }
}
