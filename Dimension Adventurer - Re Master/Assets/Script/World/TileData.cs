using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DimensionAdventurer.World
{
    [CreateAssetMenu(fileName = "NewTIle", menuName = "Data/Tile")]
    public class TileData : ScriptableObject
    {
        public Material matFloor;
        public Material matLeftWall;
        public Material matCeiling;
        public Material matRightWall;
    }
}