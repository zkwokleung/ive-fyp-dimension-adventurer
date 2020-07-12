using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DimensionAdventurer.Inputs
{
    public class TeleportJoystick : Joystick
    {
        void Awake()
        {
            //Virtual joystick is only available in mobile
            if (Application.platform != RuntimePlatform.Android || GamePreference.singleton.isPlayInVR)
                Destroy(gameObject);

            SetVisible(false);
        }

        public void SetVisible(bool visible)
        {
            Color c;
            if (visible)
            {
                //Show background
                c = background.gameObject.GetComponent<Image>().color;
                c.a = 1;
                background.gameObject.GetComponent<Image>().color = c;

                //Show Handle
                c = handle.gameObject.GetComponent<Image>().color;
                c.a = 1;
                handle.gameObject.GetComponent<Image>().color = c;
            }
            else
            {
                //Hide Background
                c = background.gameObject.GetComponent<Image>().color;
                c.a = 0;
                background.gameObject.GetComponent<Image>().color = c;

                //Hide Handle
                c = handle.gameObject.GetComponent<Image>().color;
                c.a = 0;
                handle.gameObject.GetComponent<Image>().color = c;
            }
        }
    }
}