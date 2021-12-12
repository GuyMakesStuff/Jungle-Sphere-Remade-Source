using System.Collections.Generic;
using JungleSphereRemake.Gameplay;
using UnityEngine;

namespace JungleSphereRemake.Managers
{
    public class SpawnManager : Manager<SpawnManager>
    {
        [Header("Trees")]
        public GameObject TreePrefab;
        public Transform TreeContainer;
        public float TreeSpacing;
        public float TreeStartPos;
        int TreeSpawnCount;

        [Header("Obstacles")]
        public float SpawnRange;
        public float BaseSpawnDelay;
        float SpawnDelay;
        public Transform ObstacleContainer;
        float SpawnTimer;
        [System.Serializable]
        public class ObstacleType
        {
            public string Name;
            public GameObject Prefab;
            public float StartSpawnZPos;
            [HideInInspector]
            public bool CanSpawn;
            [HideInInspector]
            public Transform Container;

            public void CreateContainer(Transform BaseContainer)
            {
                Transform NewContainer = new GameObject(Name).transform;
                NewContainer.SetParent(BaseContainer);
                NewContainer.position = Vector3.zero;
                Container = NewContainer;
            }
        }
        public ObstacleType[] Obstacles;
        List<ObstacleType> ObstaclesThatCanSpawn;

        // Start is called before the first frame update
        void Start()
        {
            Init(this);

            InitializeTrees();
            InitializeObstacles();
            ObstaclesThatCanSpawn = new List<ObstacleType>();
            SpawnTimer = BaseSpawnDelay;
            UpdateSpawnDelay();
        }
        void InitializeTrees()
        {
            float TreePos = TreeStartPos;

            while(TreePos < 260f)
            {
                SpawnTree(TreePos);
                TreePos += TreeSpacing;
            }
        }
        void InitializeObstacles()
        {
            foreach (ObstacleType O in Obstacles)
            {
                O.CreateContainer(ObstacleContainer);
            }
        }

        // Update is called once per frame
        void Update()
        {
            foreach (ObstacleType O in Obstacles)
            {
                if(GameManager.ZPos > O.StartSpawnZPos && !O.CanSpawn)
                {
                    O.CanSpawn = true;
                    ObstaclesThatCanSpawn.Add(O);
                }
            }

            UpdateSpawnDelay();
            SpawnTimer -= Time.deltaTime;
            if (SpawnTimer <= 0f && !GameManager.TentSpawned)
            {
                SpawnTimer = SpawnDelay;
                Vector3 SpawnPos = new Vector3(Random.Range(-SpawnRange, SpawnRange), 0.5f, 260f);
                int ObstacleIndex = Random.Range(0, ObstaclesThatCanSpawn.Count);
                GameObject NewObstacle = Instantiate(ObstaclesThatCanSpawn[ObstacleIndex].Prefab, SpawnPos, Quaternion.identity);
                NewObstacle.transform.SetParent(ObstaclesThatCanSpawn[ObstacleIndex].Container);
            }
        }

        void UpdateSpawnDelay()
        {
            SpawnDelay = BaseSpawnDelay / GameManager.Instance.ForwardSpeed;
        }

        public MovingObject SpawnTree(float Pos)
        {
            GameObject NewTree = (Instantiate(TreePrefab, new Vector3(0, 0, Pos), Quaternion.identity, TreeContainer));
            NewTree.name = "Tree_" + (TreeSpawnCount + 1).ToString("000");
            TreeSpawnCount++;
            return NewTree.GetComponent<MovingObject>();
        }
    }
}