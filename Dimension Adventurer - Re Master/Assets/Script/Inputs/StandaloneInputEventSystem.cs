using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Inputs
{
    public class StandaloneInputEventSystem : MonoBehaviour
    {
        private static StandaloneInputEventSystem singleton;

        public static event Action LeftButtonClickedEvent;
        public static event Action RightButtonClickedEvent;
        public static event Action UpButtonClickedEvent;
        public static event Action DownButtonClickedEvent;
        public static event Action TeleportButtonClickedEvent;
        public static event Action ChargeButtonClickedEvent;
        public static event Action EscClickedEvent;

        void Awake()
        {
            if (singleton == null)
                singleton = this;
            else if (singleton != this)
                Destroy(this);

            if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.WindowsPlayer)
                Destroy(this);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (EscClickedEvent != null)
                    EscClickedEvent.Invoke();
                else
                    GameManager.Paused = !GameManager.Paused;
            }

            if (Input.GetButtonDown("Left button"))
                if (LeftButtonClickedEvent != null)
                    LeftButtonClickedEvent.Invoke();

            if (Input.GetButtonDown("Right button"))
                if (RightButtonClickedEvent != null)
                    RightButtonClickedEvent.Invoke();

            if (Input.GetButtonDown("Up button"))
                if (UpButtonClickedEvent != null)
                    UpButtonClickedEvent.Invoke();

            if (Input.GetButtonDown("Down button"))
                if (DownButtonClickedEvent != null)
                    DownButtonClickedEvent.Invoke();

            if (Input.GetButtonDown("Teleport button"))
                if (TeleportButtonClickedEvent != null)
                    TeleportButtonClickedEvent.Invoke();

            if (Input.GetButtonDown("Charge button"))
                if (ChargeButtonClickedEvent != null)
                    ChargeButtonClickedEvent.Invoke();
        }
    }
}