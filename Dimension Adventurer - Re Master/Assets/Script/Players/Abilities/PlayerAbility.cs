using DimensionAdventurer.Players.Modifiers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Players.Abilities
{
    /// <summary>
    /// The ability of the player moving and teleporting.
    /// </summary>
    public class PlayerAbility : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Teleport teleport;
        private IPlayerMovementPerformer playerMovement;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip teleportAudio;

        private bool _teleportMode;
        public bool TeleportMode
        {
            get => _teleportMode;
            set
            {
                if (_teleportMode != value)
                {
                    _teleportMode = value;
                    teleport.ShowIndicator(value);
                }
            }
        }

        #region MonoBehaviour
        private void Start()
        {
            playerMovement = new ITweenPlayerMovement(player);
            TeleportTo(new WorldPosition(PlaneType.Floor, RunningTrack.Middle));
        }
        #endregion

        #region Move
        public void MoveLeft()
        {
            if (_teleportMode) return;
            playerMovement.MoveLeft();
        }

        public void MoveRight()
        {
            if (_teleportMode) return;
            playerMovement.MoveRight();
        }
        #endregion

        #region Teleport
        public void SelectUp()
        {
            if (!_teleportMode) return;
            teleport.IndicatorUp();
        }

        public void SelectDown()
        {
            if (!_teleportMode) return;
            teleport.IndicatorDown();
        }

        public void SelectLeft()
        {
            if (!_teleportMode) return;
            teleport.IndicatorLeft();
        }

        public void SelectRight()
        {
            if (!_teleportMode) return;
            teleport.IndicatorRight();
        }

        public void SelectTrack(RunningTrack track)
        {
            if (!_teleportMode) return;
            teleport.TeleportTrack = track;
        }

        public void InvokeTeleport()
        {
            playerMovement.Teleport(new WorldPosition(teleport.TeleportPlane, teleport.TeleportTrack));
            audioSource.PlayOneShot(teleportAudio);
            TeleportMode = false;
            teleport.ShowIndicator(false);
        }

        public void TeleportTo(WorldPosition position)
        {
            playerMovement.Teleport(position);
            audioSource.PlayOneShot(teleportAudio);
        }
        #endregion
    }
}