using System;
using UnityEngine;

namespace DimensionAdventurer.Players.Abilities
{
    public class Teleport : MonoBehaviour
    {
        [SerializeField] private EnvironmentData environmentData;
        [SerializeField] private Player player;

        [Header("Indicator")]
        [SerializeField] private GameObject IndicatorPrefab;
        [SerializeField] private float offset = 0.1f;

        //Properties
        /// <summary>
        /// The floor that will be teleport to. 
        /// Directly set this property and the indicator will be adjusted accordingly. 
        /// </summary>
        public PlaneType TeleportPlane
        {
            get { return _teleportFloor; }
            set
            {
                _teleportFloor = value;
                PreviewPlane(value);
                TeleportTrack = RunningTrack.Middle;
            }
        }

        /// <summary>
        /// The track that will be running on after the teleport.
        /// Use IndicatorUp() or IndicatorDown() instead then try to change this variable if not controlling mobile input. 
        /// </summary>
        public RunningTrack TeleportTrack
        {
            get { return _teleportTrack; }
            set
            {
                _teleportTrack = value;
                PreviewTrack(value);
            }
        }

        //Field
        private PlaneType _teleportFloor = PlaneType.Floor;
        private RunningTrack _teleportTrack = RunningTrack.Middle;
        private GameObject _indicateLine;

        #region MonoBehaviour
        private void Awake()
        {
            if (_indicateLine == null)
                SpawnIndicator();

            TeleportPlane = player.WorldPosition.Plane;
            TeleportTrack = player.WorldPosition.Track;
            PreviewPlane(TeleportPlane);
            PreviewTrack(TeleportTrack);
            ShowIndicator(false);
        }

        void Start()
        {
            _teleportFloor = PlaneType.Floor;
            _teleportTrack = RunningTrack.Middle;
        }

        void Update()
        {

        }

        #endregion

        #region Private Method
        private void SpawnIndicator()
        {
            _indicateLine = Instantiate(IndicatorPrefab, null) as GameObject;
            Vector3 promptScale = new Vector3(environmentData.TileLength / 3f / 10f, 1, environmentData.TileLength * environmentData.MaxTile / 10f);
            _indicateLine.transform.localScale = promptScale;
            _indicateLine.transform.localPosition = environmentData.ConvertEnumToPosition(_teleportFloor, 0.1f);
            _indicateLine.SetActive(false);
        }

        private void PreviewPlane(PlaneType floor)
        {
            _indicateLine.transform.position = environmentData.ConvertEnumToPosition(floor, offset);
            Quaternion rot = new Quaternion();
            rot.eulerAngles = new Vector3(0, 0, WorldPosition.ConverEnumToRotation(TeleportPlane));
            _indicateLine.transform.localRotation = rot;
        }

        private void PreviewTrack(RunningTrack track)
        {
            switch (TeleportPlane)
            {
                case PlaneType.Floor:
                    _indicateLine.transform.localPosition = new Vector3(environmentData.ConvertEnumToPosition(track), _indicateLine.transform.localPosition.y, 0);
                    break;

                case PlaneType.Ceiling:
                    _indicateLine.transform.localPosition = new Vector3(-environmentData.ConvertEnumToPosition(track), _indicateLine.transform.localPosition.y, 0);
                    break;

                case PlaneType.LeftWall:
                    _indicateLine.transform.localPosition = new Vector3(_indicateLine.transform.localPosition.x, -environmentData.ConvertEnumToPosition(track), 0);
                    break;

                case PlaneType.RightWall:
                    _indicateLine.transform.localPosition = new Vector3(_indicateLine.transform.localPosition.x, environmentData.ConvertEnumToPosition(track), 0);
                    break;
            }
            _indicateLine.SetActive(true);
        }
        #endregion

        #region Public Method
        public void ShowIndicator(bool active)
        {
            _indicateLine.SetActive(active);
        }

        public void IndicatorUp()
        {
            // When the LEFT plane is selected
            if (TeleportPlane == player.WorldPosition.NextPlane(RotateDirection.Clockwise)) 
            {
                if (TeleportTrack != RunningTrack.Left)
                    TeleportTrack = TeleportTrack - 1;
                else
                    TeleportTrack = RunningTrack.Right;
            }
            else
            {
                // When the Right plane is selected
                if (TeleportTrack != RunningTrack.Right) 
                    TeleportTrack = TeleportTrack + 1;
                else
                    TeleportTrack = RunningTrack.Left;
            }
        }

        public void IndicatorDown()
        {
            if (TeleportPlane == player.WorldPosition.NextPlane(RotateDirection.Clockwise))
            {
                // When the LEFT plane is selected
                if (TeleportTrack != RunningTrack.Right)
                    TeleportTrack = TeleportTrack + 1;
                else
                    TeleportTrack = RunningTrack.Left;
            }
            else 
            {
                // When the Right plane is selected
                if (TeleportTrack != RunningTrack.Left)
                    TeleportTrack = TeleportTrack - 1;
                else
                    TeleportTrack = RunningTrack.Right;
            }
        }

        public void IndicatorLeft()
        {
            TeleportPlane = player.WorldPosition.NextPlane(RotateDirection.Clockwise);
        }

        public void IndicatorRight()
        {
            TeleportPlane = player.WorldPosition.NextPlane(RotateDirection.AntiClockwise);
        }
        #endregion
    }
}