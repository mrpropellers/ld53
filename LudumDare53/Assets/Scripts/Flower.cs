using System.Collections;
using UnityEngine;

namespace LeftOut.LudumDare
{
    public class Flower : MonoBehaviour
    {
        [SerializeField]
        Pollen Pollen;
        
        [SerializeField]
        Transform FlowerCenter;
        
        [SerializeField]
        float m_SpawnRadius = 15;
        Vector2 m_SpawnRadiusVector;

        [SerializeField]
        float m_SpawnRadiusDistance = 5;

        static Terrain m_Terrain;

        MaterialPropertyBlock m_PropBlock;
        [SerializeField]
        Renderer m_FlowerRenderer;
        static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        public bool ShouldAnimate { get; private set; }

        public Transform LandingPointCenter => FlowerCenter;
        // Start is called before the first frame update
        void Start()
        {
            m_SpawnRadiusVector = new Vector2(m_SpawnRadius, m_SpawnRadius);

            SetPropBlock();
            SetColor(Pollen);

            // TODO: not this
            if (ShouldAnimate)
            {
                StartCoroutine(SpawnScale(2f, Vector3.one)); // * Random.Range(0.8f, 1.2f)));
            }
            FlowerCenter.gameObject.SetActive(!ShouldAnimate);
        }

        void SetPropBlock()
        {
            m_PropBlock = new MaterialPropertyBlock();
            m_FlowerRenderer.SetPropertyBlock(m_PropBlock);
        }

        void SetColor(Pollen pollen)
        {
            Pollen = pollen;
            Debug.Log($"Set flower color to {pollen.GetNameFromColor()}");

            // TODO: why is this happening
            if (m_PropBlock == null)
            {
                SetPropBlock();
            }
            m_FlowerRenderer.GetPropertyBlock(m_PropBlock);
            m_PropBlock.SetColor(BaseColor, Pollen.Color);
            m_FlowerRenderer.SetPropertyBlock(m_PropBlock);
        }

        public void ReceivePollen(Pollen incomingPollen)
        {
            Debug.Log($"{incomingPollen.GetNameFromColor()} received on flower.");
            if (!Pollen.VerifyPollination(Pollen, incomingPollen)) return;
            var newColor = this.CrossPollinate(Pollen, incomingPollen);
            SetColor(newColor);

            // TODO: randomize or same flower shape?
            var pos = new Vector2(transform.position.x, transform.position.z);
            var points = FastPoissonDiskSampling.Sampling(pos - m_SpawnRadiusVector, pos + m_SpawnRadiusVector, m_SpawnRadiusDistance);
            foreach (var p in points)
            {
                SpawnNewFlower(gameObject, newColor, new Vector3(p.x, 0, p.y), m_Terrain);
            }
        }
        
        public void TakeOff()
        {
            StartCoroutine(DespawnScale(5f));
        }

        IEnumerator DespawnScale(float duration)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, (elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            Destroy(gameObject);
        }

        public Pollen GivePollen()
        {
            Debug.Log($"Giving some {Pollen.GetNameFromColor()} pollen to bee!");
            return new Pollen(Pollen.Color, this);
        }
        
        public static void SpawnNewFlower(GameObject flowerPrefab, Pollen pollen, Vector3 pos, Terrain terrain, bool shouldAnim = true)
        {
            m_Terrain = terrain;
            var y = m_Terrain.SampleHeight(pos);

            var flowerObj = Instantiate(flowerPrefab);
            var flower = flowerObj.GetComponent<Flower>();
            flower.SetColor(pollen);
            flower.ShouldAnimate = shouldAnim;
            flowerObj.transform.position = new Vector3(pos.x, y - Random.Range(0, 5f), pos.z);
            var terrainData = terrain.terrainData;
            var norm = terrain.terrainData.GetInterpolatedNormal(pos.x / terrainData.size.x,
                pos.z / terrainData.size.z);
            flowerObj.transform.Rotate(0, Random.Range(0, 360f), 0);
            flowerObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, norm);
        }

        IEnumerator SpawnScale(float duration, Vector3 goalScale)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, goalScale, (elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            FlowerCenter.gameObject.SetActive(true);
        }
    }
}
