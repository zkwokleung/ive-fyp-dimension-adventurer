using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.World
{
    public class Tile : MonoBehaviour, IPooledObject
    {
        [SerializeField] private EnvironmentData environmentData;
        [SerializeField] private TileData data;
        [SerializeField] private GameObject p_floor;
        [SerializeField] private GameObject p_leftWall;
        [SerializeField] private GameObject p_ceiling;
        [SerializeField] private GameObject p_rightWall;

        [HideInInspector] public WorldObjectManager objectManager;

        private void Awake()
        {
            UpdateMaterial();
            //Set transform
            p_floor.transform.localScale = new Vector3(environmentData.TileWidth, 1, environmentData.TileLength);
            p_leftWall.transform.localScale = new Vector3(1, environmentData.TileWidth, environmentData.TileLength);
            p_ceiling.transform.localScale = new Vector3(environmentData.TileWidth, 1, environmentData.TileLength);
            p_rightWall.transform.localScale = new Vector3(1, environmentData.TileWidth, environmentData.TileLength);

            p_floor.transform.localPosition = new Vector3(0, -environmentData.TileWidth / 2, 0);
            p_leftWall.transform.localPosition = new Vector3(-environmentData.TileWidth / 2, 0, 0);
            p_ceiling.transform.localPosition = new Vector3(0, environmentData.TileWidth / 2, 0);
            p_rightWall.transform.localPosition = new Vector3(environmentData.TileWidth / 2, 0, 0);

            p_floor.transform.localEulerAngles = new Vector3(0, 0, WorldPosition.ConverEnumToRotation(PlaneType.Floor));
            p_leftWall.transform.localEulerAngles = Vector3.zero;
            p_ceiling.transform.localEulerAngles = new Vector3(0, 0, WorldPosition.ConverEnumToRotation(PlaneType.Ceiling));
            p_rightWall.transform.localEulerAngles = Vector3.zero;

            if (data != null)
                UpdateMaterial();
        }

        public void OnDisable()
        {
            if (objectManager == null)
                return;

            objectManager.Tiles.Remove(gameObject);
            objectManager = null;
        }

        public void ApplyData(TileData data)
        {
            this.data = data;
            UpdateMaterial();
        }

        private void UpdateMaterial()
        {
            if (data == null)
                return;

            p_floor.GetComponent<MeshRenderer>().material = data.matFloor;
            p_leftWall.GetComponent<MeshRenderer>().material = data.matLeftWall;
            p_ceiling.GetComponent<MeshRenderer>().material = data.matCeiling;
            p_rightWall.GetComponent<MeshRenderer>().material = data.matRightWall;
        }

        public void OnObjectSpawn()
        {
            //Clear the items/traps inside the tile
            if (transform.childCount > 4)
            {
                for (int i = 4; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}