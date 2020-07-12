using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static iTween;

namespace DimensionAdventurer.Players.Abilities
{
    public class ITweenPlayerMovement : IPlayerMovementPerformer
    {
        private Player player;
        /// <summary>
        /// The amont of time needed to teleport
        /// </summary>
        [SerializeField] private float tpTime = 0.75f;
        /// <summary>
        /// Changing this will affect the animation of players' teleportation style
        /// </summary>
        /// <value>
        ///   Recommended: 
        ///   easeInBack
        ///   easeInOutQuad
        ///   easeOutQuint
        ///   easeInSine                                                                                          
        /// </value>
        [SerializeField] private EaseType tpEase = EaseType.easeInOutQuad;

        /// <summary>
        /// The offset of the player to the floor
        /// </summary>
        public float offsetToFloor { get; private set; } = 4f;
        /// <summary>
        /// The speed of the player moving left and right.
        /// </summary>
        public float speed { get; private set; } = 1f;

        public ITweenPlayerMovement(Player player)
        {
            this.player = player;
        }

        public void MoveLeft()
        {
            WorldPosition newPos = player.WorldPosition;
            if (player.WorldPosition.Track != RunningTrack.Left)
                player.WorldPosition.Track = (RunningTrack)((int)player.WorldPosition.Track - 1);
            else
                return;
            MoveTo(player.WorldPosition);
        }

        public void MoveRight()
        {
            if (player.WorldPosition.Track != RunningTrack.Right)
                player.WorldPosition.Track = (RunningTrack)((int)player.WorldPosition.Track + 1);
            else
                return;
            MoveTo(player.WorldPosition);
        }

        public void Teleport(WorldPosition worldPosition)
        {
            player.WorldPosition = worldPosition;

            //rotate
            iTween.RotateTo(player.gameObject, iTween.Hash(
                "z", WorldPosition.ConverEnumToRotation(worldPosition.Plane),
                "time", tpTime,
                "easetype", tpEase
                ));

            //Move
            iTween.MoveTo(player.gameObject, iTween.Hash(
                "position", GameManager.singleton.EnvironmentData.ConvertEnumToPosition(worldPosition, offsetToFloor),
                "time", tpTime,
                "easetype", tpEase
                ));
        }

        private void MoveTo(WorldPosition worldPosition)
        {
            iTween.MoveTo(player.gameObject, iTween.Hash(
                "position", GameManager.singleton.EnvironmentData.ConvertEnumToPosition(worldPosition, offsetToFloor),
                "time", 0.5f)
                );
        }

        private IEnumerator MoveTo(WorldPosition worldPosition, float seconds)
        {
            iTween.MoveTo(player.gameObject, iTween.Hash(
                "position", GameManager.singleton.EnvironmentData.ConvertEnumToPosition(worldPosition, offsetToFloor),
                "time", seconds)
                );
            yield return new WaitForSeconds(seconds);
            player.WorldPosition = worldPosition;
        }
    }

}