using UnityEngine;

namespace LeftOut.Channels
{
    [CreateAssetMenu(fileName = "FloatChannel", menuName = "Left Out/Float Channel", order = 0)]
    public class FloatChannel : ScriptableObject
    {
        [field: SerializeField]
        public float Value { get; set; }
    }
}
