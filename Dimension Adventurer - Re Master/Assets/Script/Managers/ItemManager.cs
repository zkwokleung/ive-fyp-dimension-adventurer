using System.Collections.Generic;
using UnityEngine;
using DimensionAdventurer.Items;
using System;
using Random = UnityEngine.Random;

namespace DimensionAdventurer.Managers
{
    public class ItemManager : ManagerMonoBehaviour
    {
        public static ItemManager singleton;

        [SerializeField] private List<ItemData> itemDatas;

        #region Attribute
        /// <summary>
        /// The number of items that can be spawned on a single tile.
        /// Default is 3. Please adjust according to the tile length or the item size.
        /// </summary>
        [SerializeField] private int itemsOnEachTile = 3;

        /// <summary>
        /// Changing the item spawn path for the number range of tiles spawned
        /// </summary>
        [SerializeField] private Vector2Int pathChangeOnTileRange;
        #endregion

        #region Runtime
        private int _pathChangeCountdown;
        private WorldPosition _spawnPath;
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

            //Get environment Attribute
            container.Items = new List<GameObject>();

            //Creating the list of special items
            if (ItemData.SpecialItems == null)
                ItemData.SpecialItems = new List<string>();
            foreach(ItemData id in itemDatas)
                if (id.isSpecialItem)
                    ItemData.SpecialItems.Add(id.name);

            _pathChangeCountdown = environmentData.itemSpawnPathChangeInterval;
            _spawnPath = WorldPosition.RandomPosition();
        }

        private void OnEnable()
        {
            TileManager.singleton.TileSpawnEvent += OnTileSpawn;
            TileManager.singleton.TileRemoveEvent += OnTileRemove;
        }
        #endregion

        #region Item Managing
        /// <summary>
        /// Spawn a group of items on the given tile
        /// </summary>
        public void SpawnItemGroup(GameObject parentTile)
        {
            GameObject temp;

            //Distance between
            Vector3 step = new Vector3(0, 0, environmentData.TileLength / itemsOnEachTile);

            //Set the starting position to the edge of the tile
            Vector3 spawnPos = parentTile.transform.position - new Vector3(0, 0, environmentData.TileLength / 2);
            spawnPos += environmentData.ConvertEnumToPosition(_spawnPath, environmentData.itemOffsetToGround);

            for(int i = 0; i < itemsOnEachTile; i++)
            {
                spawnPos += step;
                temp = SpawnItem(spawnPos);
                temp.transform.SetParent(parentTile.transform);
            }

            _pathChangeCountdown--;
        }

        /// <summary>
        /// Spawn 1 random item
        /// </summary>
        private GameObject SpawnItem(Vector3 pos)
        {
            GameObject newItem;

            //Random
            int random = Random.Range(0, 100);
            if (random > environmentData.spItemSpawnRate)
            {
                //Spawn coin
                newItem = ObjectPool.singleton.Spawn("Coin", pos);
            }
            else
            {
                //Spawn random special item
                newItem = ObjectPool.singleton.Spawn(ItemData.SpecialItems[Random.Range(0, ItemData.SpecialItems.Count)], pos);
            }

            return newItem;
        }

        /// <summary>
        /// Randomly select a path for items to spawn.
        /// </summary>
        private void RandomPath()
        {
            //Random the item spawn path
            _spawnPath = WorldPosition.RandomPosition();

            //Reset the path change countdown
            _pathChangeCountdown = Random.Range(pathChangeOnTileRange.x, pathChangeOnTileRange.y);
        }
        #endregion

        #region Events
        private void OnTileSpawn(GameObject obj)
        {
            //Check if the spawn path needs to be changed
            if (_pathChangeCountdown <= 0)
                RandomPath();

            SpawnItemGroup(obj);
        }

        private void OnTileRemove(GameObject obj)
        {

        }
        #endregion
    }
}