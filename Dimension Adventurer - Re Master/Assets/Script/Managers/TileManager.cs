using DimensionAdventurer.Players;
using DimensionAdventurer.World;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DimensionAdventurer.Managers
{
    public class TileManager : ManagerMonoBehaviour
    {
        public static TileManager singleton;

        #region User Control Attribute
        /// <summary>
        /// An array that stores all tile data
        /// </summary>
        public TileData[] tileDatas;
        /// <summary>
        /// An array of all portals prefabs
        /// </summary>
        public GameObject[] portalPrefabs;
        /// <summary>
        /// Controls that should the game scene start moving. 
        /// For situation like: openning animation, game over
        /// </summary>
        public bool tileEnabled = false;
        /// <summary>
        /// The number of tiles from the begining that would not spawn any traps or items
        /// </summary>
        public int emptyTileOnStart;
        public float movingSpeedMultiplier = 1f;
        /// <summary>
        /// Change the tile set for every this amount of scores gained.
        /// </summary>
        public float tileChangeOnScore = 15000;
        #endregion

        #region Runtime attributes
        private int _tileDataIndex;
        /// <summary>
        /// How many times did the tile style been changed.
        /// </summary>
        private int _tileChangedCount;
        /// <summary>
        /// Is the tile changing.
        /// </summary>
        private bool _changingTile;
        /// <summary>
        /// The moving speed of tiles and other scene objects
        /// </summary>
        private float _movingSpeed;
        /// <summary>
        /// The acceleration magnitude of moving speed over-time
        /// </summary>
        private float _movingSpeedAcceleration = 0.1f;
        /// <summary>
        /// The position where the tile spawn.
        /// </summary>
        private Vector3 _spawnPosition;
        #endregion

        #region Events
        public event Action<GameObject> TileSpawnEvent;
        public event Action<GameObject> TileRemoveEvent;
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

            _tileChangedCount = 1;
            _spawnPosition = new Vector3(0, 0, environmentData.TileLength * -5);
            _movingSpeed = environmentData.MinMovingSpeed;
        }

        private void OnEnable()
        {
            //Event
            GameManager.PauseEvent += OnPauseResume;
            GameManager.GameStartEvent += OnGameStart;
            GameManager.GameOverEvent += OnGameOver;
        }

        private void OnDisable()
        {
            //Event
            GameManager.PauseEvent -= OnPauseResume;
            GameManager.GameStartEvent -= OnGameStart;
            GameManager.GameOverEvent -= OnGameOver;
        }

        void Start()
        {
            //Random a tile data on start
            _tileDataIndex = Random.Range(0, tileDatas.Length);

            //Initialize game scene
            for (int i = 0; i < environmentData.MaxTile; i++)
            {
                if (i < 3)
                    SpawnTile(_tileDataIndex);
                _spawnPosition.z += environmentData.TileLength;
            }
        }

        void Update()
        {
            if (!tileEnabled)
                return;

            //Move tiles
            TileMove();

            //Spawn and removal of tiles
            if (ShouldTileSpawn())
                SpawnTile(_tileDataIndex);
            else
            {
                // Calculate distance between player and the oldest tile
                float distance = -container.Tiles[0].transform.localPosition.z;

                // Remove when the distance is greater than the user define distance
                if (distance >= environmentData.RemoveDistance)
                    RemoveTile(container.Tiles[0]);
            }

            //Change Tile set
            Player[] ps = GameManager.GetAllPlayer();
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].Score > (tileChangeOnScore * _tileChangedCount))
                {
                    ChangeTileData();
                    _tileChangedCount++;
                }
            }

            //Updating the moving speed overtime
            if (_movingSpeed < environmentData.MaxMovingSpeed)
                _movingSpeed += Time.deltaTime * _movingSpeedAcceleration;
        }

        #endregion

        #region Tile Management
        /// <summary>
        /// Returns a boolean if the scene should spawn a new tile.
        /// Seperated from Update() for easier maintenance.
        /// </summary>
        private bool ShouldTileSpawn() => container.Tiles.Count < environmentData.MaxTile;


        private void SpawnTile(int dataIndex)
        {
            GameObject newTile = ObjectPool.singleton.Spawn("Tile", _spawnPosition);
            newTile.GetComponent<Tile>().ApplyData(tileDatas[dataIndex]);
            newTile.GetComponent<Tile>().objectManager = this.container;
            container.Tiles.Add(newTile);

            //Spawn portal
            if (_changingTile)
            {
                SpawnPortal(newTile);
                _changingTile = false;
            }

            // Invoke Events
            if (emptyTileOnStart <= 0)
                TileSpawnEvent.Invoke(newTile);
            else
                emptyTileOnStart--;
        }

        private void RemoveTile(GameObject tile)
        {
            ObjectPool.singleton.Despawn(tile);

            if (TileRemoveEvent != null)
                TileRemoveEvent.Invoke(tile);

            container.Tiles.Remove(tile);
        }

        private void TileMove()
        {
            //Move the first tile
            container.Tiles[0].transform.localPosition -= container.Tiles[0].transform.forward * _movingSpeed * Time.deltaTime * movingSpeedMultiplier;
            //Debug.Log(game.Tiles[0].transform.localPosition.ToString());

            //Move other tiles base on the position of their previous tile
            for (int i = 1; i < container.Tiles.Count; i++)
            {
                Vector3 newPosition = container.Tiles[i - 1].transform.localPosition;
                newPosition.z += environmentData.TileLength;
                container.Tiles[i].transform.localPosition = newPosition;
            }//end for loop
        }

        private void ChangeTileData()
        {
            //Do not change the tile when there is only one prefabs
            if (tileDatas.Length < 2)
                return;

            int newIdx = Random.Range(0, tileDatas.Length);

            //Recursively call when  the number is the same.
            if (newIdx == _tileDataIndex)
                ChangeTileData();
            else
                _tileDataIndex = newIdx;

            _changingTile = true;
        }

        private void SpawnPortal(GameObject parent)
        {
            if (portalPrefabs.Length < 1)
            {
                Debug.LogError("No portal prefab set");
            }

            Vector3 pos = _spawnPosition;
            pos.z -= environmentData.TileLength / 2;
            GameObject portal = Instantiate(portalPrefabs[Random.Range(0, portalPrefabs.Length)], pos, Quaternion.identity) as GameObject;
            portal.transform.SetParent(parent.transform);
        }
        #endregion

        #region Event
        public void OnGameStart()
        {
            tileEnabled = true;
        }

        public void OnGameOver(string reason)
        {
            tileEnabled = false;
        }

        public void OnPauseResume(bool isPause)
        {
            if (!GamePreference.IsMultiplayer)
                tileEnabled = !isPause;
        }
        #endregion
    }
}

