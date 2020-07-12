using System.Collections.Generic;
using UnityEngine;
using DimensionAdventurer.Players;
using DimensionAdventurer.Players.Modifiers.Statuses;
using DimensionAdventurer.Players.Modifiers;

namespace DimensionAdventurer.UI
{
    public class UIStatusDisplayList : MonoBehaviour
    {
        //Player data
        public Player Player { get; private set; }
        public int Count { get => _displayers.Count; }

        //Instantiates
        [SerializeField] private GameObject displayerPrefab;
        private Dictionary<Modifier, StatusDisplayer> _displayers;

        #region MonoBehaviour
        private void Awake()
        {
            _displayers = new Dictionary<Modifier, StatusDisplayer>();
        }

        void OnEnable()
        {
            GameManager.PlayerSpawnEvent += OnPlayerSpawn;
        }

        private void OnDisable()
        {
            GameManager.PlayerSpawnEvent -= OnPlayerSpawn;
        }
        #endregion

        #region List
        public void Register(Modifier modifier)
        {
            StatusDisplayer displayer;

            if (_displayers.ContainsKey(modifier))
            {
                displayer = _displayers[modifier];
                displayer.gameObject.SetActive(true);
            }
            else
            {
                displayer = SpawnDisplayer();
                _displayers.Add(modifier, displayer);
            }

            displayer.Initialize(modifier);
        }

        public void Unregister(Modifier modifier)
        {

        }

        private StatusDisplayer SpawnDisplayer()
        {
            GameObject newDis = Instantiate(displayerPrefab, gameObject.transform) as GameObject;
            StatusDisplayer displayer = newDis.GetComponent<StatusDisplayer>();
            return displayer;
        }
        #endregion

        #region Event
        public void OnModifierInvoke(object source, ModifierEventArgs e)
        {
            if(e.action == ModifierEventArgs.Action.Add)
                Register(e.Modifier);
        }   

        public void OnPlayerSpawn(PlayerSpawnEventArgs e)
        {
            if (e.isLocalPlayer)
                this.Player = e.player;

            Player.ModifierEvent += OnModifierInvoke;
        }
        #endregion
    }
}