using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer
{
    public enum RunningTrack
    {
        Left = 0,
        Middle = 1,
        Right = 2,
        Translating = 8080
    }

    public enum PlaneType
    {
        Floor = 0,
        LeftWall = 1,
        Ceiling = 2,
        RightWall = 3,
        Translating = 8080
    }

    public enum RotateDirection
    {
        AntiClockwise = 1,
        Clockwise = -1
    }

    public enum Port
    {
        SinglePlayer = 7777,
        Multiplayer = 6333
    }

}