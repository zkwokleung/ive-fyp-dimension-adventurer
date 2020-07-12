using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DimensionAdventurer.Inputs
{
    public class TeleportInputMobile : MonoBehaviour
    {
        /// <summary>
        /// Define the touch zone of the screen by percentage to active teleport
        /// </summary>
        protected static float SCREEN_TOUCH_ZONE_PERCENTAGE = 0.2f;

        private void Awake()
        {
            // Disable this script if the platform was not mobile
            if (Application.platform != RuntimePlatform.Android || Application.platform != RuntimePlatform.IPhonePlayer)
                this.enabled = false;
        }

        private void Update()
        {
            

        }
    }
}