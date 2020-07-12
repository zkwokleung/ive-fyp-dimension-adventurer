using System;
using UnityEngine;

namespace DimensionAdventurer.World
{
    public class VFXPortal : Portal
    {
        [SerializeField] private GameObject[] Lines;

        void Start()
        {
            for (int i = 0; i < 4; i++)
            {
                Lines[i].transform.localPosition = EnvironmentData.ConvertEnumToPosition((PlaneType)i);
                Lines[i].transform.localEulerAngles = new Vector3(0, 0, WorldPosition.ConverEnumToRotation((PlaneType)i));

                switch ((PlaneType)i)
                {
                    case PlaneType.Floor:
                        Lines[i].transform.localPosition += new Vector3(0, .5f, 0);
                        break;

                    case PlaneType.LeftWall:
                        Lines[i].transform.localPosition += new Vector3(.5f, 0, 0);
                        break;

                    case PlaneType.Ceiling:
                        Lines[i].transform.localPosition += new Vector3(0, -.5f, 0);
                        break;

                    case PlaneType.RightWall:
                        Lines[i].transform.localPosition += new Vector3(-.5f, 0, 0);
                        break;
                }
            }

        }
    }
}