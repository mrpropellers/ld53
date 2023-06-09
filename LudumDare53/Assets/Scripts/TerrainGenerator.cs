using UnityEngine;

namespace LeftOut.LudumDare
{
    // https://www.youtube.com/watch?v=vFvwyu_ZKfU
    public class TerrainGenerator : MonoBehaviour
    {
        [Header("Terrain settings")]
        [SerializeField]
        int m_Width = 256;
        [SerializeField]
        int m_Height = 25;
        [SerializeField]
        int m_Length = 256;
        [SerializeField]
        float m_Scale = 20f;
        [SerializeField]
        AnimationCurve m_CliffCurve;

        [SerializeField]
        bool m_RandomizeOffsets = true;
        [SerializeField]
        float m_OffX = 100f;
        [SerializeField]
        float m_OffY = 100f;

        Terrain m_Terrain;

        [Header("Flower generation settings")]
        [SerializeField]
        GameObject[] m_FlowerPrefabs;
        [SerializeField]
        float m_MinDistance = 2f;

        void Start()
        {
            if (m_RandomizeOffsets)
            {
                m_OffX = Random.Range(0f, 9999f);
                m_OffY = Random.Range(0f, 9999f);
            }
            m_Terrain = GetComponent<Terrain>();
            m_Terrain.terrainData = GenerateTerrain(m_Terrain.terrainData);

            GenerateFlowers();
        }

        TerrainData GenerateTerrain(TerrainData terrainData)
        {
            terrainData.heightmapResolution = m_Width + 1;
            terrainData.size = new Vector3(m_Width, m_Height, m_Length);

            terrainData.SetHeights(0, 0, GenerateHeights());
            return terrainData;
        }

        float[,] GenerateHeights()
        {
            var heights = new float[m_Width, m_Length];
            for (int x = 0; x < m_Width; x++)
            {
                for (int y = 0; y < m_Length; y++)
                {
                    heights[x, y] = CalculateHeight(x, y);
                }
            }


            return heights;
        }

        float RatioToCenter(float x, float y)
        {
            var distance = new Vector2(x - m_Width / 2f, y - m_Length / 2f).magnitude;
            return distance / Mathf.Min(m_Width/2f, m_Length/2f);
        }

        float CalculateHeight(int x, int y)
        {
            var xCoord = (float)x / m_Width * m_Scale + m_OffX;
            var yCoord = (float)y / m_Length * m_Scale + m_OffY;

            return Mathf.Clamp01(Mathf.PerlinNoise(xCoord, yCoord) * m_CliffCurve.Evaluate(RatioToCenter(x, y)));
        }

        void GenerateFlowers()
        {
            //var parent = new GameObject("FlowerParent").transform;
            var points = FastPoissonDiskSampling.Sampling(Vector2.zero, new Vector2(m_Width, m_Length), m_MinDistance);
            int counter = 0;
            foreach (var p in points)
            {
                // Check for elevation
                var p3 = new Vector3(p.x, 0, p.y);
                var y = m_Terrain.SampleHeight(p3);

                // 1/2 or above
                if (y > m_Height / 2)
                {
                    // Rand skip 6/10
                    if (Random.Range(0, 11) < 7)
                    {
                        continue;
                    }
                }

                // 3/4 or above
                if (y > 3 * m_Height / 4)
                {
                    // Rand skip 9/10
                    if (Random.Range(0, 11) < 9)
                    {
                        continue;
                    }
                }

                // Select and place flower
                var randPrimary = Pollen.HexList[Random.Range(0, Pollen.HexList.Length)];
                Flower.SpawnNewFlower(m_FlowerPrefabs[counter % m_FlowerPrefabs.Length], new Pollen(randPrimary.GetColor(), null), p3, m_Terrain, shouldAnim: false);
                counter++;
            }
        }
    }
}
