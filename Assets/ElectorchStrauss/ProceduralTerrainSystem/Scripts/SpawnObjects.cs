using UnityEngine;
using Random = UnityEngine.Random;

namespace ElectorchStrauss.ProceduralTerrainSystem.Scripts
{
    public class SpawnObjects : MonoBehaviour
    {
        public GameObject[] algueaPrefab = new GameObject[3];
        public GameObject[] grassPrefab = new GameObject[3];
        public GameObject[] rockPrefab = new GameObject[3];
        public GameObject[] treePrefab = new GameObject[2];
        public GameObject[] deadTreePrefab = new GameObject[1];
        public int nbrOfAlguea;
        public int nbrOfGrass;
        public int nbrOfRock;
        public int nbrOfTree;
        public int nbrOfDeadTree;
        public float terrainRange = 500f;
        private GameObject rockParent, treeParent, grassParent, algueaParent;

        private void Start()
        {
            rockParent = new GameObject("RockParent");
            treeParent = new GameObject("TreeParent");
            grassParent = new GameObject("GrassParent");
            algueaParent = new GameObject("AlgueaParent");
        }

        public void SpawnThem()
        {
            SpawnAlgueaPrefab();
            SpawnGrassPrefab();
            SpawnRockPrefab();
            SpawnDeadTreePrefab();
            SpawnTreePrefab();
        }
        public void DeleteThem()
        {
            int childs = rockParent.transform.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                DestroyImmediate(rockParent.transform.GetChild(i).gameObject);
            }

            int childs1 = treeParent.transform.childCount;
            for (int i = childs1 - 1; i >= 0; i--)
            {
                DestroyImmediate(treeParent.transform.GetChild(i).gameObject);
            }

            int childs2 = grassParent.transform.childCount;
            for (int i = childs2 - 1; i >= 0; i--)
            {
                DestroyImmediate(grassParent.transform.GetChild(i).gameObject);
            }

            int childs3 = algueaParent.transform.childCount;
            for (int i = childs3 - 1; i >= 0; i--)
            {
                DestroyImmediate(algueaParent.transform.GetChild(i).gameObject);
            }   
        }

        void SpawnAlgueaPrefab()
        {
            for (int i = 0; i < nbrOfAlguea; i++)
            {
                foreach (var t in algueaPrefab)
                {
                    float randomX = Random.Range(-terrainRange, terrainRange);
                    float randomZ = Random.Range(-terrainRange, terrainRange);
                    Ray ray = new Ray(new Vector3(randomX, terrainRange, randomZ),
                        new Vector3(randomX, -terrainRange, randomZ));
                    if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform.gameObject.layer == 4)
                    {
                        Debug.DrawRay(new Vector3(randomX, terrainRange, randomZ),
                            new Vector3(randomX, -terrainRange, randomZ) * hit.distance, Color.blue);
                        if (Physics.Raycast(hit.point,
                            hit.point - new Vector3(randomX, terrainRange, randomZ), out RaycastHit hit1))
                        {
                            if (hit1.transform.gameObject.layer == 3 &&
                                Vector3.Dot(Vector3.up, hit.normal) >= Mathf.Cos(95f * Mathf.Deg2Rad))
                            {
                                Debug.DrawRay(new Vector3(randomX, terrainRange, randomZ),
                                    new Vector3(randomX, -terrainRange, randomZ) * hit1.distance, Color.red);
                                var alguea = Instantiate(t, hit1.point,
                                    Quaternion.LookRotation(t.transform.forward, hit1.normal));
                                alguea.transform.SetParent(algueaParent.transform);
                                alguea.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
                            }
                        }
                    }
                }
            }
        }

        void SpawnGrassPrefab()
        {
            for (int i = 0; i < nbrOfGrass; i++)
            {
                foreach (var t in grassPrefab)
                {
                    float randomX = Random.Range(-terrainRange, terrainRange);
                    float randomZ = Random.Range(-terrainRange, terrainRange);
                    if (Physics.Raycast(new Vector3(randomX, terrainRange, randomZ),
                            new Vector3(randomX, -terrainRange, randomZ), out RaycastHit hit) &&
                        hit.transform.gameObject.layer == 3)
                    {
                        if (Vector3.Dot(Vector3.up, hit.normal) >= Mathf.Cos(45f * Mathf.Deg2Rad))
                        {
                            Debug.DrawRay(new Vector3(randomX, terrainRange, randomZ),
                                new Vector3(randomX, -terrainRange, randomZ), Color.green);
                            var grass = Instantiate(t, hit.point,
                                Quaternion.LookRotation(t.transform.forward, hit.normal));
                            grass.transform.SetParent(grassParent.transform);
                            grass.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
                        }
                    }
                }
            }
        }

        void SpawnRockPrefab()
        {
            for (int i = 0; i < nbrOfRock; i++)
            {
                foreach (var t in rockPrefab)
                {
                    float randomX = Random.Range(-terrainRange, terrainRange);
                    float randomZ = Random.Range(-terrainRange, terrainRange);
                    if (Physics.Raycast(new Vector3(randomX, terrainRange, randomZ),
                            new Vector3(randomX, -terrainRange, randomZ), out RaycastHit hit) &&
                        hit.transform.gameObject.layer == 3)
                    {
                        if (Vector3.Dot(Vector3.up, hit.normal) >= Mathf.Cos(65f * Mathf.Deg2Rad))
                        {
                            Debug.DrawRay(new Vector3(randomX, terrainRange, randomZ),
                                new Vector3(randomX, -terrainRange, randomZ), Color.black);

                            var rock = Instantiate(t, hit.point,
                                Quaternion.LookRotation(t.transform.forward, hit.normal));
                            rock.transform.SetParent(rockParent.transform);
                            rock.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
                        }
                    }
                }
            }
        }

        void SpawnTreePrefab()
        {
            for (int j = 0; j < 1; j++)
            {
                foreach (var t in treePrefab)
                {
                    for (int i = 0; i < nbrOfTree; i++)
                    {
                        float randomX = Random.Range(-terrainRange, terrainRange);
                        float randomZ = Random.Range(-terrainRange, terrainRange);
                        if (Physics.Raycast(new Vector3(randomX, terrainRange, randomZ),
                            new Vector3(randomX, -terrainRange, randomZ), out RaycastHit hit))
                        {
                            if (hit.transform.gameObject.layer == 3)
                            {
                                if (hit.point.y is < 60 and > 13)
                                {
                                    if (Vector3.Dot(Vector3.up, hit.normal) >= Mathf.Cos(45f * Mathf.Deg2Rad))
                                    {
                                        Debug.DrawRay(new Vector3(randomX, terrainRange, randomZ),
                                            new Vector3(randomX, -terrainRange, randomZ), Color.red);

                                        var tree = Instantiate(t, hit.point,
                                            Quaternion.LookRotation(t.transform.forward, hit.normal));
                                        tree.transform.SetParent(treeParent.transform);
                                        tree.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void SpawnDeadTreePrefab()
        {
            for (int i = 0; i < nbrOfDeadTree; i++)
            {
                foreach (var t in deadTreePrefab)
                {
                    float randomX = Random.Range(-terrainRange, terrainRange);
                    float randomZ = Random.Range(-terrainRange, terrainRange);
                    if (Physics.Raycast(new Vector3(randomX, terrainRange, randomZ),
                            new Vector3(randomX, -terrainRange, randomZ), out RaycastHit hit) &&
                        hit.transform.gameObject.layer == 3)
                    {
                        if (Vector3.Dot(Vector3.up, hit.normal) >= Mathf.Cos(45f * Mathf.Deg2Rad) &&
                            hit.point.y > 55)
                        {
                            Debug.DrawRay(new Vector3(randomX, terrainRange, randomZ),
                                new Vector3(randomX, -terrainRange, randomZ), Color.gray);

                            var deadtree = Instantiate(t, hit.point,
                                Quaternion.LookRotation(t.transform.forward, hit.normal));
                            deadtree.transform.SetParent(treeParent.transform);
                            deadtree.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
                        }
                    }
                }
            }
        }
    }
}