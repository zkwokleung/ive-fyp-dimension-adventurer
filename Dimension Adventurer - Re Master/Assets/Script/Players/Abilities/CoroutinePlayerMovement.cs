using System;
using System.Collections;
using UnityEngine;

/* OBSOLETE
namespace DimensionAdventurer.Players.Abilities
{
    [Obsolete("Recommend using ITween instead")]
    /// <summary>
    ///  Moving the character to move with corotuine
    /// </summary>
    public class CoroutinePlayerMovement : IPlayerMovementPerformer
    {
        private PlayerControl playerControl;
        private IEnumerator CurrentCoroutine;

        public CoroutinePlayerMovement(PlayerControl playerControl)
        {
            this.playerControl = playerControl;
            CurrentCoroutine = null;
        }

        public void MoveLeft()
        {
            //Move Left
            if (playerControl.Player.WorldPosition.Track != RunningTrack.Left)
                Translate((RunningTrack)((int)playerControl.Player.WorldPosition.Track - 1));
        }

        public void MoveRight()
        {
            //Move Right
            if (playerControl.Player.WorldPosition.Track != RunningTrack.Right)
                Translate((RunningTrack)((int)playerControl.Player.WorldPosition.Track + 1));
        }

        private void Translate(RunningTrack target)
        {
            if (CurrentCoroutine != null)
                playerControl.StopCoroutine(CurrentCoroutine);

            CurrentCoroutine = ITranslate(target);
            playerControl.StartCoroutine(CurrentCoroutine);
        }


        private void RotateTowards
            (RotateDirection rotateDirection)
        {
            if (CurrentCoroutine != null)
                playerControl.StopCoroutine(CurrentCoroutine);

            CurrentCoroutine = IRotateTowards(rotateDirection);
            playerControl.StartCoroutine(CurrentCoroutine);
        }

        public void Teleport(WorldPosition worldPosition)
        {
            Debug.Log("Teleport");
            if (CurrentCoroutine != null)
                playerControl.StopCoroutine(CurrentCoroutine);

            if (worldPosition.Plane != playerControl.Player.WorldPosition.Plane)
            {
                CurrentCoroutine = ITeleport(worldPosition.Plane, worldPosition.Track);
                playerControl.StartCoroutine(CurrentCoroutine);
            }
            else
            {
                CurrentCoroutine = ITranslate(worldPosition.Track);
                playerControl.StartCoroutine(CurrentCoroutine);
            }
        }


        #region IEnumerators
        private IEnumerator ITeleport(PlaneType floor, RunningTrack track)
        {
            //Wall jump
            if (playerControl.Player.WorldPosition.NextPlane(RotateDirection.AntiClockwise) == floor)
                yield return IRotateTowards(RotateDirection.AntiClockwise);
            else
                yield return IRotateTowards(RotateDirection.Clockwise);

            yield return ITranslate(track);
        }
        /// <summary>
        /// Running track overload.
        /// </summary>
        private IEnumerator ITranslate(RunningTrack runningTrack)
        {
            playerControl.Player.WorldPosition.Track = runningTrack;
            Vector3 target;
            //Movement according to the plane standing
            switch (playerControl.Player.WorldPosition.Plane)
            {
                case PlaneType.Ceiling:
                    target = new Vector3(-playerControl.EnvironmentData.ConvertEnumToPosition(runningTrack), 0, 0);
                    break;

                case PlaneType.Floor:
                    target = new Vector3(playerControl.EnvironmentData.ConvertEnumToPosition(runningTrack), 0, 0);
                    break;

                case PlaneType.LeftWall:
                    target = new Vector3(0, -playerControl.EnvironmentData.ConvertEnumToPosition(runningTrack), playerControl.transform.position.z);
                    break;

                case PlaneType.RightWall:
                    target = new Vector3(0, playerControl.EnvironmentData.ConvertEnumToPosition(runningTrack), 0);
                    break;

                default:
                    target = playerControl.transform.position;
                    break;
            }
            target += playerControl.EnvironmentData.ConvertEnumToPosition(playerControl.Player.WorldPosition.Plane, playerControl.offsetToFloor);

            yield return ITranslate(target);
        }
        /// <summary>
        /// Translate to target position.
        /// </summary>
        /// <param name="target">Position of the player after translated</param>
        private IEnumerator ITranslate(Vector3 target)
        {
            while (Vector3.Distance(playerControl.transform.position, target) > 0.05f)
            {
                playerControl.transform.position = Vector3.Lerp(playerControl.transform.position, target, playerControl.speed * Time.deltaTime);
                yield return null;
            }
        }
        /// <summary>
        /// Rotate the aspect of the player.
        /// </summary>
        private IEnumerator IRotateTowards(RotateDirection rotateDirection)
        {
            //Start Rotation
            do
            {
                playerControl.transform.Rotate(Vector3.forward * (int)rotateDirection * playerControl.EnvironmentData.RotateSpeed);
                playerControl.Player.WorldPosition.Plane = playerControl.CheckCurrentPlane(playerControl.transform.eulerAngles);
                yield return new WaitForSeconds(0.01f);
            } while (playerControl.Player.WorldPosition.Plane == PlaneType.Translating);


        }
        #endregion

    }
}
*/