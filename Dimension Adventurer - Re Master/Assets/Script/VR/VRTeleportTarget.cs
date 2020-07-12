using DimensionAdventurer.Players;
using DimensionAdventurer.Players.Abilities;
using DimensionAdventurer.Players.Controls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.VR
{
    public class VRTeleportTarget : MonoBehaviour
    {
        [SerializeField] private EnvironmentData EnvironmentData;
        [SerializeField] private WorldPosition wPosition;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _selectedMaterial;

        private MeshRenderer _meshRenderer;
        private PlayerAbility ability;

        private void Awake()
        {
            //Set transform
            gameObject.transform.localEulerAngles = new Vector3(0, 0, WorldPosition.ConverEnumToRotation(wPosition.Plane));
            gameObject.transform.localPosition = EnvironmentData.ConvertEnumToPosition(wPosition);
            gameObject.transform.localScale = new Vector3(EnvironmentData.TileWidth / 3f, 0.1f, 1f);

            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            GameManager.PlayerSpawnEvent += OnPlayerSpawn;
        }

        private void OnDisable()
        {
            GameManager.PlayerSpawnEvent -= OnPlayerSpawn;
        }

        void Start()
        {

        }

        public void OnGazeEnter()
        {
            SetSelected(true);
        }

        public void OnGazeExit()
        {
            SetSelected(false);
        }

        public void OnGazeComplete()
        {
            SetSelected(false);
            TeleportPlayer();
        }

        public void SetSelected(bool selected)
        {
            if (selected)
                _meshRenderer.material = _selectedMaterial;
            else
                _meshRenderer.material = _defaultMaterial;
        }

        public void TeleportPlayer()
        {
            ability.TeleportTo(wPosition);
        }

        #region Event
        public void OnPlayerSpawn(PlayerSpawnEventArgs e)
        {
            if (e.isLocalPlayer)
                ability = e.player.gameObject.GetComponent<PlayerAbility>();
        }
        #endregion
    }

}