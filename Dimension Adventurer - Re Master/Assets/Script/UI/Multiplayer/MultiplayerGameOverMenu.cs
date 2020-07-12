using DimensionAdventurer.Players;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DimensionAdventurer.UI.Multiplayers
{
    public class MultiplayerGameOverMenu : MonoBehaviour
    {
        private const string REMARK_PLAYER_WIN = "Win";
        private const string REMARK_PLAYER_LOSE = "Lose";
        private const string REMARK_PLAYER_DC = "Disconnected";

        [SerializeField] private MultiplayerGameOverPlayerInfo mgoLocal;
        [SerializeField] private MultiplayerGameOverPlayerInfo mgoRemote;
        [SerializeField] private TextMeshProUGUI txtRemarkLocal;
        [SerializeField] private TextMeshProUGUI txtRemarkRemote;

        private void OnEnable()
        {
            Player localPlayer = GameManager.LocalPlayer;
            Player remotePlayer = GameManager.GetPlayer(GameManager.REMOTE_PLAYER_ID);
            mgoLocal.Player = localPlayer;
            mgoRemote.Player = remotePlayer;

            // If the game over is not cause by disconnection of the other player.
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                // Local player win
                if (localPlayer.Score > remotePlayer.Score)
                {
                    txtRemarkLocal.text = REMARK_PLAYER_WIN;
                    txtRemarkRemote.text = REMARK_PLAYER_LOSE;
                    txtRemarkLocal.color = Color.green;
                    txtRemarkRemote.color = Color.red;
                }
                // Remote Player win
                else
                {
                    txtRemarkLocal.text = REMARK_PLAYER_LOSE;
                    txtRemarkRemote.text = REMARK_PLAYER_WIN;
                    txtRemarkLocal.color = Color.red;
                    txtRemarkRemote.color = Color.green;
                }
            }
            else
            {
                // If the other player left, the local player wins.
                txtRemarkLocal.text = REMARK_PLAYER_WIN;
                txtRemarkRemote.text = REMARK_PLAYER_DC;
                txtRemarkLocal.color = Color.green;
                txtRemarkRemote.color = Color.red;
            }
        }
    }
}
