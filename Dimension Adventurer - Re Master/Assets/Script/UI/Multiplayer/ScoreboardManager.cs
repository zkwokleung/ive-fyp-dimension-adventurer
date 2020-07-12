using DimensionAdventurer.Players;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.UI.Multiplayers
{
    public class ScoreboardManager : MonoBehaviour
    {
        [Header("Scoreboard Tag")]
        [SerializeField] private PlayerScoreboard psLocal;
        [SerializeField] private PlayerScoreboard psRemote;

        // For Re-ordering the name tag base on the score
        //[SerializeField] private Transform FirstPlaceTransform;
        //[SerializeField] private Transform SecondPlayerTransform;

        #region MonoBehaviour
        public void OnEnable()
        {
            GameManager.PlayerSpawnEvent += OnPlayerSpawn;
        }

        public void OnDisable()
        {
            GameManager.PlayerSpawnEvent -= OnPlayerSpawn;
        }

        private void Start()
        {
            if (!GamePreference.IsMultiplayer)
                gameObject.SetActive(false);
            else
            {
                foreach(KeyValuePair<string, Player> p in GameManager.players)
                {
                    if (p.Key != PhotonNetwork.NickName)
                    {
                        psRemote.Player = p.Value;
                    }
                    //Debug.Log($"Player Count: {PlayerManager.players.Count}");
                }
            }
        }
        #endregion

        #region Event
        private void OnPlayerSpawn(PlayerSpawnEventArgs e)
        {
            if (e.isLocalPlayer)
                psLocal.Player = e.player;
            else
                psRemote.Player = e.player;
        }
        #endregion
    }
}