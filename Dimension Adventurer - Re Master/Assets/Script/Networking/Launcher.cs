using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Random = UnityEngine.Random;

namespace DimensionAdventurer.Networking
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public const string PASSWORD_MATCH_PREFIX = "PRIVATE_";
        public static Launcher singleton;
        public static event Action<string> LogFeedbackEvent;

        #region Fields
        public byte MaxPlayerPerMatch = 2;
        public string GameVersion = "1.0.0";
        public bool IsConnecting { get; private set; } = false;

        private string _password = "";
        #endregion

        #region MonoBehaviour
        private void Awake()
        {
            // Singleton
            if (singleton == null)
                singleton = this;
            else if (singleton != this)
            {
                Debug.Log($"{GetType().Name}: Instance already exists, destroying object . . . ");
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            IsConnecting = false;
            if (PhotonNetwork.IsConnected)
                PhotonNetwork.Disconnect();
        }
        #endregion

        #region Public Functions
        public void CreateRoom(string password = "")
        {
            _password = password;

            RoomOptions options = new RoomOptions
            {
                MaxPlayers = this.MaxPlayerPerMatch
            };

            if (_password == string.Empty)
            {
                // Quick Match
                LogFeedback(NetLogMessage.CREATING_MATCH);
            }
            else
            {
                // Password Match
                // The match will not be public for other players if password is set
                options.IsVisible = false;
                LogFeedback(NetLogMessage.CREATING_PASSWORD_MATCH);
            }

            // Using the password as the room name
            PhotonNetwork.CreateRoom(_password, options);
        }

        public void Connect(string playerName = "")
        {
            //If there is no player name entered, generate a random player name
            playerName = (playerName == string.Empty) ? RandomPlayerName() : playerName;
            PhotonNetwork.LocalPlayer.NickName = playerName;

            LogFeedback(NetLogMessage.CONNECTING);
            PhotonNetwork.GameVersion = GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        public void JoinRandomMatch()
        {
            LogFeedback(NetLogMessage.JOINING_MATCH);
            PhotonNetwork.JoinRandomRoom();
        }

        public void JoinPasswordMatch(string password)
        {
            LogFeedback(NetLogMessage.JOINING_MATCH);
            _password = PASSWORD_MATCH_PREFIX + password;
            PhotonNetwork.JoinRoom(_password);
        }

        public void LogFeedback(string message)
        {
            Debug.Log($"Launcher: {message}");
            if (LogFeedbackEvent != null)
                LogFeedbackEvent.Invoke(message);
        }
        #endregion


        #region Pun Callback
        public override void OnConnectedToMaster()
        {
            Debug.Log($"Launcher: Connected to server with nickname: {PhotonNetwork.NickName}");
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Launcher: Joinned lobby");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            LogFeedback(NetLogMessage.JOIN_RANDOM_FAIL);
            CreateRoom();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            LogFeedback(NetLogMessage.JOIN_PASSWORD_FAIL);
            CreateRoom(_password);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Launcher: Disconnected from server");
            IsConnecting = false;
        }

        public override void OnJoinedRoom()
        {
                LogFeedback($"{NetLogMessage.WAITING_PLAYER}");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.LoadLevel("GameScene");
            }
        }

        public override void OnLeftLobby()
        {
            Debug.Log("Launcher: Left lobby");
        }
        #endregion

        #region Public Static functions
        public static string RandomPlayerName(string prefix = "Player", string postfix = "") => prefix + Random.Range(1000, 10000) + postfix;
        #endregion
    }
}