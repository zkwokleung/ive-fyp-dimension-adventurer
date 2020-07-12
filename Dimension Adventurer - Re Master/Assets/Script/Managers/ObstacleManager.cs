using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Managers
{
    public class ObstacleManager : ManagerMonoBehaviour
    {
        public static ObstacleManager singleton;

        #region User Control Attributes
        [SerializeField] private GameObject[] obstaclePrefabs;
        #endregion

        #region Runtime Attributes
        [SerializeField] private float _cooldownCounter;
        #endregion

        #region MonoBehaviour
        private void Awake()
        {
            // Singleton
            if (singleton == null)
                singleton = this;
            else if (singleton != this)
            {
                Debug.Log($"{GetType().Name}: Instance already exists, destroying object . . . ");
                Destroy(gameObject);
            }

            //Initialize
            _cooldownCounter = environmentData.obstacleInitCooldown;
        }

        private void OnEnable()
        {
            TileManager.singleton.TileSpawnEvent += OnTileSpawn;
            TileManager.singleton.TileRemoveEvent += OnTileRemove;
        }

        void Update()
        {
            if (GameManager.Paused)
                return;

            _cooldownCounter -= Time.deltaTime;
        }
        #endregion

        #region Obstacle Managing
        /// <summary>
        /// Check if the system should spawn a new obstacle to the scene
        /// </summary>
        /// <param name="tile">Which tile the obstacle will on</param>
        /// <param name="spawnPosition">The spawn position of the obstacle</param>
        public void SpawnObstacle(GameObject tile)
        {
            if (_cooldownCounter > 0)
                return;

            //Random Spawn 
            int random = UnityEngine.Random.Range(0, 100);
            if (random < environmentData.obstacleSpawnRate)
            {
                //Random obstacle prefabs index
                int r = UnityEngine.Random.Range(0, obstaclePrefabs.Length);

                //Random running track for obstacle to spawn at
                RunningTrack rt = WorldPosition.RandomTrack();

                //The actual position where the obstacle spawn
                Vector3 pos = new Vector3(environmentData.ConvertEnumToPosition(rt), environmentData.obstacleOffset, tile.transform.position.z);

                GameObject newObstacle = ObjectPool.singleton.Spawn("Obstacle", pos);
                newObstacle.transform.SetParent(tile.transform);
                container.Obstacles.Add(newObstacle);

                //Reset cooldown
                _cooldownCounter = environmentData.obstacleNormalCooldown;
            }//endif random < spawnRate
        }

        public void PopObstacle(GameObject parentTile)
        {
            if (container.Obstacles.Count <= 0)
                return;

            if (container.Obstacles[0].transform.IsChildOf(parentTile.transform))
            {
                container.Obstacles[0].SetActive(false);
                container.Obstacles.RemoveAt(0);
            }
        }

        #endregion

        #region Event
        private void OnTileSpawn(GameObject obj)
        {
            SpawnObstacle(obj);
        }
        private void OnTileRemove(GameObject obj)
        {
            PopObstacle(obj);
        }
        #endregion
    }
}
