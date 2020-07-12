using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.VR
{
    public class VRTeleportManager : MonoBehaviour
    {
        void Start()
        {
            if (!GamePreference.singleton.isPlayInVR)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }
    }
}