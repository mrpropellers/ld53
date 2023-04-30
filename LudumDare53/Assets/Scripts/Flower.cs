using UnityEngine;

namespace LeftOut.LudumDare
{
    public class Flower : MonoBehaviour
    {
        [SerializeField]
        Color PollenColor;
        
        [SerializeField]
        Transform FlowerCenter;

        public Transform LandingPointCenter => FlowerCenter;
        // Start is called before the first frame update
        void Start()
        {
        
        }
        
        // Update is called once per frame
        void Update()
        {
        
        }

        public void ReceivePollen(Pollen incomingPollen)
        {
            Debug.Log($"{incomingPollen.Color} received.");
        }

        public Pollen GivePollen()
        {
            Debug.Log($"Giving up some {PollenColor} pollen");
            return new Pollen(PollenColor, this);
        }
    }
}
