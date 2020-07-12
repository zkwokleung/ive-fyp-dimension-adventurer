using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using DimensionAdventurer.Inputs;
using static iTween;
using DimensionAdventurer.Players.Abilities;

namespace DimensionAdventurer.Players.Controls
{
    public class StandalonePlayerControl : MonoBehaviour
    {
        public PlayerAbility Ability;

        #region MonoBehaviour
        private void Awake()
        {
            // Disable the script when the gamei s not running on Windows platform.
            if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.WindowsPlayer)
                this.enabled = false;
        }

        private void OnEnable()
        {
            StandaloneInputEventSystem.LeftButtonClickedEvent += OnLeftButtonClicked;
            StandaloneInputEventSystem.RightButtonClickedEvent += OnRightButtonClicked;
            StandaloneInputEventSystem.UpButtonClickedEvent += OnUpButtonClicked;
            StandaloneInputEventSystem.DownButtonClickedEvent += OnDownButtonClicked;
            StandaloneInputEventSystem.TeleportButtonClickedEvent += OnTeleportButtonClicked;
        }

        private void OnDisable()
        {
            StandaloneInputEventSystem.LeftButtonClickedEvent -= OnLeftButtonClicked;
            StandaloneInputEventSystem.RightButtonClickedEvent -= OnRightButtonClicked;
            StandaloneInputEventSystem.UpButtonClickedEvent -= OnUpButtonClicked;
            StandaloneInputEventSystem.DownButtonClickedEvent -= OnDownButtonClicked;
            StandaloneInputEventSystem.TeleportButtonClickedEvent -= OnTeleportButtonClicked;
        }

        private void Start()
        {
            Ability.TeleportTo(new WorldPosition(PlaneType.Floor, RunningTrack.Middle));
        }
        #endregion

        #region Button Click
        private void OnLeftButtonClicked()
        {

            if (Ability.TeleportMode)
            {
                // Teleport mode
                Ability.SelectLeft();
            }
            else
            {
                // Not Teleport mode
                Ability.MoveLeft();
            }
        }

        private void OnRightButtonClicked()
        {
            if (Ability.TeleportMode)
            {
                // Teleport mode
                Ability.SelectRight();
            }
            else
            {
                // Not Teleport mode
                Ability.MoveRight();
            }
        }

        private void OnUpButtonClicked()
        {
            if (Ability.TeleportMode)
            {
                // Teleport mode
                Ability.SelectUp();
            }
            else
            {
                // Not Teleport mode
                
            }
        }

        private void OnDownButtonClicked()
        {
            if (Ability.TeleportMode)
            {
                // Teleport mode
                Ability.SelectDown();
            }
            else
            {
                // Not Teleport mode

            }
        }

        private void OnTeleportButtonClicked()
        {
            if (Ability.TeleportMode)
            {
                // Teleport mode
                Ability.InvokeTeleport();
            }
            else
            {
                // Not Teleport mode
                Ability.TeleportMode = true;
            }
        }
        #endregion
    }
}