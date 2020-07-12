using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace DimensionAdventurer
{
    public class WorldObjectManager : MonoBehaviour
    {
        /// <summary>
        /// Tiles list Spawned in the scene. SerializeField for Debuging. 
        /// </summary>
        public List<GameObject> Tiles;
        /// <summary>
        /// Contained all traps in the game scene.
        /// </summary>
        public List<GameObject> Obstacles;
        /// <summary>
        /// All items generated will be stored in this list.
        /// </summary>
        public List<GameObject> Items;

        #region MonoBehaviour
        private void Awake()
        {
            //Initilaize Lists
            Tiles = new List<GameObject>();
            Obstacles = new List<GameObject>();
            Items = new List<GameObject>();
        }
        #endregion


        #region Getters
        /// <summary>
        /// The oldest tile in the scene
        /// </summary>
        public GameObject OldestTile()
        {
            if (Tiles.Count > 0)
                return Tiles[0];
            else
                return null;
        }

        /// <summary>
        /// The oldest obstacle in the scene
        /// </summary>
        public GameObject OldestObstacle()
        {
            if (Obstacles.Count > 0)
                return Obstacles[0];
            else
                return null;
        }

        /// <summary>
        /// The oldest item in the scene
        /// </summary>
        public GameObject OldestItem()
        {
            if (Items.Count > 0)
                return Items[0];
            else
                return null;
        }

        /// <summary>
        /// The latest tile spawned in the scene.
        /// </summary>
        public GameObject LatestTile()
        {
            if (Tiles.Count > 0)
                return Tiles[Tiles.Count - 1];
            else
                return null;
        }

        /// <summary>
        /// The latest obstacle spawned in the scene.
        /// </summary>
        public GameObject LatestObstacle()
        {
            if (Obstacles.Count > 0)
                return Obstacles[Obstacles.Count - 1];
            else
                return null;
        }

        /// <summary>
        /// The latest item spawned in the scene.
        /// </summary>
        public GameObject LatestItem()
        {
            if (Items.Count > 0)
                return Items[Items.Count - 1];
            else
                return null;
        }
        #endregion
    }
}