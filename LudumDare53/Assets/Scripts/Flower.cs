using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace LeftOut.LudumDare
{
    public class Flower : MonoBehaviour
    {
        GameObject MeshType;
        bool m_CanYieldPollen;
        
        [FormerlySerializedAs("ParentPollen")]
        [FormerlySerializedAs("Pollen")]
        [SerializeField]
        Pollen PrimaryPollen;

        [SerializeField]
        Pollen SecondaryPollen;
        
        [SerializeField]
        Transform FlowerCenter;

        [SerializeField]
        float SpawnDuration = 3.5f;
        
        [SerializeField]
        float m_SpawnRadius = 15;
        Vector2 m_SpawnRadiusVector;

        [SerializeField]
        float m_SpawnRadiusDistance = 5;

        static Terrain k_Terrain;

        [SerializeField]
        Renderer m_FlowerRenderer;
        static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        static readonly int ColorA = Shader.PropertyToID("_ColorA");
        static readonly int ColorB = Shader.PropertyToID("_ColorB");
        bool ShouldAnimate { get; set; }

        public Transform LandingPointCenter => FlowerCenter;
        // Start is called before the first frame update
        void Start()
        {
            m_SpawnRadiusVector = new Vector2(m_SpawnRadius, m_SpawnRadius);
            m_CanYieldPollen = true;

            if (ShouldAnimate)
            {
                StartCoroutine(SpawnScale(SpawnDuration, Vector3.one)); // * Random.Range(0.8f, 1.2f)));
            }
            FlowerCenter.gameObject.SetActive(!ShouldAnimate);
        }

        void SetColor(Pollen primaryPollen = null, Pollen secondaryPollen = null)
        {
            if (primaryPollen != null)
            {
                Debug.Log($"Primary color is {primaryPollen.Color}");
                PrimaryPollen = primaryPollen;    
                m_FlowerRenderer.material.SetColor(ColorA, primaryPollen.Color);
                if (secondaryPollen == null)
                {
                    m_FlowerRenderer.material.SetColor(ColorB, primaryPollen.Color);
                }
            }

            if (secondaryPollen != null)
            {
                Debug.Log($"Secondary color is {secondaryPollen.Color}");
                SecondaryPollen = secondaryPollen;
                m_FlowerRenderer.material.SetColor(ColorB, secondaryPollen.Color);
            }
        }

        public bool ReceivePollen(Pollen incomingPollen)
        {
            Debug.Log($"{incomingPollen.Color} received on flower.");
            //SetColor(secondaryPollen: incomingPollen);

            var pos = new Vector2(transform.position.x, transform.position.z);
            var points = FastPoissonDiskSampling.Sampling(pos - m_SpawnRadiusVector, pos + m_SpawnRadiusVector, m_SpawnRadiusDistance);
            foreach (var p in points)
            {
                // var meshType = gameObject;
                // if (PrimaryPollen.Parent != null)
                // {
                //     meshType = PrimaryPollen.Parent.MeshType;
                // }
                SpawnNewFlower(gameObject, PrimaryPollen, new Vector3(p.x, 0, p.y), k_Terrain, secondaryPollen: incomingPollen);
            }

            return true;
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
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }

        public Pollen GivePollen()
        {
            Debug.Log($"Giving some {PrimaryPollen.Color} primaryPollen to bee!");
            return new Pollen(PrimaryPollen.Color, this);
        }
        
        public static void SpawnNewFlower(GameObject flowerPrefab, Pollen primaryPollen, Vector3 pos, Terrain terrain, bool shouldAnim = true, Pollen secondaryPollen = null)
        {
            k_Terrain = terrain;
            var y = k_Terrain.SampleHeight(pos);

            var flowerObj = Instantiate(flowerPrefab);
            var flower = flowerObj.GetComponent<Flower>();
            flower.SetColor(primaryPollen: primaryPollen, secondaryPollen: secondaryPollen);
            flower.MeshType = flowerPrefab;
            flower.ShouldAnimate = shouldAnim;
            flowerObj.transform.position = new Vector3(pos.x, y - Random.Range(2, 6f), pos.z);
            var terrainData = terrain.terrainData;
            var norm = terrain.terrainData.GetInterpolatedNormal(pos.x / terrainData.size.x,
                pos.z / terrainData.size.z);
            flowerObj.transform.Rotate(0, Random.Range(0, 360f), 0);
            flowerObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, norm);
        }

        IEnumerator SpawnScale(float duration, Vector3 goalScale)
        {
            transform.localScale = Vector3.zero;
            DOVirtual.Vector3(Vector3.zero, goalScale / 2f, duration / 2f,
                scale => transform.localScale = scale).SetEase(Ease.InCirc);
            yield return new WaitForSeconds(duration / 2f);
            DOVirtual.Vector3(goalScale / 2f, goalScale, duration / 2f,
                (current) => transform.localScale = current)
                .SetEase(Ease.OutElastic);
            yield return new WaitForSeconds(duration/2f);

            FlowerCenter.gameObject.SetActive(true);
            
        }
    }
}
