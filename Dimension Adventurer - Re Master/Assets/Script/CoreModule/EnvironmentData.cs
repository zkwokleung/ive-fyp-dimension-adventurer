using DimensionAdventurer;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer
{
    [CreateAssetMenu(menuName = "Data/Environment")]
    public class EnvironmentData : ScriptableObject
    {
        #region Tile
        /// <summary>
        /// The Maximum moving speed of tiles.
        /// </summary>
        public float MaxMovingSpeed;
        /// <summary>
        /// The starting moving speed of tiles.
        /// </summary>
        public float MinMovingSpeed;
        /// <summary>
        /// The distance between tile and player when it remove
        /// </summary>
        public float RemoveDistance;
        /// <summary>
        /// Let the script know the general length of tiles
        /// </summary>
        public float TileLength;
        /// <summary>
        /// The standard width of each tile. For calculate track/path.
        /// </summary>
        public float TileWidth;
        /// <summary>
        /// the numbers of tile allowed on the scene
        /// </summary>
        public int MaxTile;
        #endregion

        #region Obstacle
        /// <summary>
        /// The spawn rate of the obstacle for every tile
        /// </summary>
        [Range(0, 100)]
        public int obstacleSpawnRate = 10;
        /// <summary>
        /// The offest of obstacles form y-position 0
        /// </summary>
        public float obstacleOffset;
        /// <summary>
        /// The cooldown of obstacles spawning at the beginning of the game.
        /// </summary>
        public float obstacleInitCooldown = 5;
        /// <summary>
        /// The cooldown of obstacles spawning
        /// </summary>
        public float obstacleNormalCooldown = 3;
        #endregion

        #region Item
        public float itemOffsetToGround = 5f;
        public int itemSpawnPathChangeInterval = 5;
        [Range(0, 100)]
        public int spItemSpawnRate;
        #endregion

        #region Method
        /// <summary>
        /// Convert the running track to x position base on tile width.
        /// </summary>
        /// <param name="runningTrack">Enum of the running track</param>
        /// <returns>x position</returns>
        public float ConvertEnumToPosition(RunningTrack runningTrack)
        {
            switch (runningTrack)
            {
                case RunningTrack.Left:
                    return TileWidth / -3;

                case RunningTrack.Middle:
                    return 0;

                case RunningTrack.Right:
                    return TileWidth / 3;

                default:
                    return 0;
            }

            //Unity does not support expression switch
            /*
            return runningTrack switch
            {
                RunningTrack.Left => TileWidth / -3,

                RunningTrack.Middle => 0,

                RunningTrack.Right => TileWidth / 3,

                _ => 0,
            };*/
        }

        /// <summary>
        /// Overload with floor
        /// </summary>
        public Vector3 ConvertEnumToPosition(PlaneType planeType, float offset = 0)
        {
            Vector3 position;
            switch (planeType)
            {
                case (PlaneType.Floor):
                    position = new Vector3(0, -TileWidth / 2 + offset, 0);
                    break;

                case (PlaneType.Ceiling):
                    position = new Vector3(0, TileWidth / 2 - offset, 0);
                    break;

                case (PlaneType.LeftWall):
                    position = new Vector3(-TileWidth / 2 + offset, 0);
                    break;

                case (PlaneType.RightWall):
                    position = new Vector3(TileWidth / 2 - offset, 0);
                    break;

                default:
                    return Vector3.zero;
            }
            //Debug.Log(position.ToString());

            position = WorldPosition.UpDirectionOf(planeType).Value * ((-TileWidth / 2) + offset);

            return position;
        }

        public Vector3 ConvertEnumToPosition(PlaneType planeType, RunningTrack runningTrack, float offset = 0)
        {
            Vector3 pos = ConvertEnumToPosition(planeType, offset);

            switch (planeType)
            {
                case PlaneType.Ceiling:
                    pos.x = -ConvertEnumToPosition(runningTrack);
                    break;

                case PlaneType.Floor:
                    pos.x = ConvertEnumToPosition(runningTrack);
                    break;

                case PlaneType.LeftWall:
                    pos.y = -ConvertEnumToPosition(runningTrack);
                    break;

                case PlaneType.RightWall:
                    pos.y = ConvertEnumToPosition(runningTrack);
                    break;
            }

            return pos;
        }

        public Vector3 ConvertEnumToPosition(WorldPosition worldPosition, float offset = 0)
        {
            return ConvertEnumToPosition(worldPosition.Plane, worldPosition.Track, offset);
        }

        public Vector3 RandomPosition(float offset = 0)
        {
            /* OBSOLETE
            float posX = 0f;
            float posY = 0f;
            //Generate position again if both position is middle
            do
            {
                posX = ConvertEnumToPosition(WorldPosition.RandomTrack());
                posY = ConvertEnumToPosition(WorldPosition.RandomTrack());
            } while (posX == ConvertEnumToPosition(RunningTrack.Middle) && posY == ConvertEnumToPosition(RunningTrack.Middle));
            return new Vector3(posX, posY, posZ);
            */

            return ConvertEnumToPosition(WorldPosition.RandomPlane(), WorldPosition.RandomTrack(), offset);

        }
        #endregion
    }
}