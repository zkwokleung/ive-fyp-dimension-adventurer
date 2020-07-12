using UnityEngine;

namespace DimensionAdventurer
{
    [System.Serializable]
    public class WorldPosition
    {
        public PlaneType Plane;
        public RunningTrack Track;

        public Vector3? Up { get => UpDirectionOf(this.Plane); }
        
        //Constructor
        public WorldPosition(PlaneType plane, RunningTrack track)
        {
            Plane = plane;
            Track = track;
        }

        public bool CompareTo(WorldPosition worldPosition) => ((Plane == worldPosition.Plane && Track == worldPosition.Track));

        public bool CompareTo(RunningTrack track) => Track == track;

        public bool CompareTo(PlaneType plane) => Plane == plane;

        /// <summary>
        /// Return the next plane base on the current plane and the rotate direction
        /// </summary>
        /// <param name="rotateDirection"></param>
        /// <returns></returns>
        public PlaneType NextPlane(RotateDirection rotateDirection)
        {
            if (rotateDirection == RotateDirection.AntiClockwise)
            {
                switch (Plane)
                {
                    case PlaneType.Floor:
                        return PlaneType.RightWall;

                    case PlaneType.Ceiling:
                        return PlaneType.LeftWall;

                    case PlaneType.LeftWall:
                        return PlaneType.Floor;

                    case PlaneType.RightWall:
                        return PlaneType.Ceiling;
                }//end switch
            }//end if
            else
            {
                switch (Plane)
                {
                    case PlaneType.Floor:
                        return PlaneType.LeftWall;

                    case PlaneType.Ceiling:
                        return PlaneType.RightWall;

                    case PlaneType.LeftWall:
                        return PlaneType.Ceiling;

                    case PlaneType.RightWall:
                        return PlaneType.Floor;
                }//end switch
            }//end else
            return PlaneType.Floor;
        }


        #region Overriding
        public override string ToString()
        {
            return string.Format("Plane: {0}, Track: {1}", Plane.ToString(), Track.ToString());
        }

        public override int GetHashCode()
        {
            string code = ((int)Plane).ToString() + ((int)Track).ToString();
            return int.Parse(code);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as WorldPosition);
        }

        public bool Equals(WorldPosition pos)
        {
            return (Plane == pos.Plane) && (Track == pos.Track);
        }
        #endregion



        #region Operator
        //Operator overloading
        public static bool operator ==(WorldPosition wP1, WorldPosition wP2)
        {
            return wP1.Plane == wP2.Plane && wP1.Track == wP2.Track;
        }

        public static bool operator !=(WorldPosition wP1, WorldPosition wP2)
        {
            return wP1.Plane != wP2.Plane || wP1.Track != wP2.Track;
        }




        #endregion



        #region Static Functions
        /// <summary>
        /// Return a random track
        /// </summary>
        /// <returns>Random RunningTrack</returns>
        public static RunningTrack RandomTrack()
        {
            return (RunningTrack)Random.Range(0, 3);
        }

        /// <summary>
        /// Return a random plane
        /// </summary>
        /// <returns>Random Plane</returns>
        public static PlaneType RandomPlane()
        {
            return (PlaneType)Random.Range(0, 4);
        }

        public static WorldPosition RandomPosition()
        {
            return new WorldPosition(RandomPlane(), RandomTrack());
        }

        public static float ConverEnumToRotation(PlaneType plane)
        {
            switch (plane)
            {
                case (PlaneType.Floor):
                    return 0f;

                case (PlaneType.LeftWall):
                    return -90f;

                case (PlaneType.Ceiling):
                    return -180f;

                case (PlaneType.RightWall):
                    return 90f;

                default:
                    return 0;
            }
        }

        public static Vector3? UpDirectionOf(PlaneType plane)
        {
            switch (plane)
            {
                case PlaneType.Floor:
                    return Vector3.up;

                case PlaneType.LeftWall:
                    return Vector3.right;

                case PlaneType.Ceiling:
                    return Vector3.down;

                case PlaneType.RightWall:
                    return Vector3.left;

                default:
                    return null;
            }
        }
        #endregion
    }
}
