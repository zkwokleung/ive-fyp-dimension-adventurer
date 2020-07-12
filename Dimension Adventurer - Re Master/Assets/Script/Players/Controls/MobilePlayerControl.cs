using DimensionAdventurer.Inputs;
using DimensionAdventurer.Players.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Players.Controls
{
    public class MobilePlayerControl : MonoBehaviour
    {
        private const float TELEPORT_TRIGGER_ZONE = 0.2f;

        [SerializeField] private PlayerAbility playerAbility;

        private float teleportZoneLeft = 0f;
        private float teleportZoneRight = 0f;
        private float teleportZoneDown = 0f;
        private float teleportZoneUp = 0f;

        private void Awake()
        {
            // If not playing on mobile
            if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.WindowsEditor)
                this.enabled = false;
        }

        private void OnEnable()
        {
            teleportZoneLeft = Screen.width * TELEPORT_TRIGGER_ZONE;
            teleportZoneRight = Screen.width - teleportZoneLeft;
            teleportZoneDown = Screen.height / 3f;
            teleportZoneUp = Screen.height - teleportZoneDown;

            MobileInputEventSystem.MobileInputEvent += OnMobileInput;
        }

        private void OnDisable()
        {
            MobileInputEventSystem.MobileInputEvent -= OnMobileInput;
        }

        private void OnMobileInput(MobileInputEventArgs e)
        {
            if (e.gesture == Gesture.Swipe)
                OnSwipe(e);
            else if (e.gesture == Gesture.Drag)
                OnDrag(e);
            else if (e.gesture == Gesture.DragEnd)
                OnDragEnd(e);
            else if (e.gesture == Gesture.DoubleTap)
                OnDoubleTap(e);
        }

        private void OnSwipe(MobileInputEventArgs e)
        {
            if (e.startPos.x > teleportZoneLeft && e.startPos.x < teleportZoneRight)
            {
                if (e.position.x < e.startPos.x)
                {
                    playerAbility.MoveLeft();
                }
                else if (e.position.x > e.startPos.x)
                {
                    playerAbility.MoveRight();
                }
            }
        }

        private void OnDrag(MobileInputEventArgs e)
        {
            // If not start inside teleport input zone
            if (e.startPos.x > teleportZoneLeft && e.startPos.x < teleportZoneRight)
                return;

            if (!playerAbility.TeleportMode)
                playerAbility.TeleportMode = true;

            // Left
            if (e.position.x <= teleportZoneLeft)
                playerAbility.SelectLeft();
            // Right
            else if (e.position.x >= teleportZoneRight)
                playerAbility.SelectRight();

            // Up
            if (e.position.y <= teleportZoneDown)
                playerAbility.SelectDown();
            // Down
            else if (e.position.y >= teleportZoneUp)
                playerAbility.SelectUp();
            else
                playerAbility.SelectTrack(RunningTrack.Middle);

        }

        private void OnDragEnd(MobileInputEventArgs e)
        {
            if (playerAbility.TeleportMode)
                playerAbility.InvokeTeleport();
        }

        private void OnDoubleTap(MobileInputEventArgs e)
        {

        }
    }
}