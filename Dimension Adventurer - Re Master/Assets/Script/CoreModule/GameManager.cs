using DimensionAdventurer;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using DimensionAdventurer.Players;
using DimensionAdventurer.VR;
using DimensionAdventurer.UI;
using System;
using Photon.Pun;
using Photon.Realtime;
using DimensionAdventurer.Networking;
using Player = DimensionAdventurer.Players.Player;
using ExitGames.Client.Photon;

namespace DimensionAdventurer
{
    public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        #region Static Player Management
        public const string LOCAL_PLAYER_ID = "LocalPlayer";
        public const string REMOTE_PLAYER_ID = "RemotePlayer";

        public static Player LocalPlayer { get; private set; }
        public static Dictionary<string, Player> players = new Dictionary<string, Player>();

        public static void RegisterPlayer(string playerId, Player Player)
        {
            players.Add(playerId, Player);
        }

        public static void UnregisterPlayer(string playerId)
        {
            players.Remove(playerId);
        }

        public static int PlayerAlive { get; private set; }

        public static Player GetPlayer(string playerId)
        {
            return players[playerId];
        }

        public static Player[] GetAllPlayer()
        {
            return players.Values.ToArray();
        }
        #endregion

        #region Game Over Reason
        public const string PLAYER_LEFT = "The other player has left.";
        public const string ALL_PLAYER_DEAD = "All player is dead.";
        #endregion

        public static GameManager singleton { get; private set; }
        private static bool _paused = false;
        public static bool Paused
        {
            get => _paused;
            set
            {
                _paused = value;
                if (!GamePreference.IsMultiplayer)
                    Time.timeScale = (_paused) ? 0f : 1f;
                if (PauseEvent != null)
                    PauseEvent.Invoke(_paused);
            }
        }
        public static bool IsGameOver { get; private set; }
        public static bool IsGameRunning { get; private set; }

        /// <summary>
        /// Invoke when the game starts
        /// </summary>
        public static event Action GameStartEvent;

        /// <summary>
        /// Invoke when the game is over.
        /// </summary>
        public static event Action<string> GameOverEvent;

        /// <summary>
        /// Invoke when the game is paused or resume.
        /// </summary>
        public static event Action<bool> PauseEvent;

        /// <summary>
        /// Invoke when a player is spawned.
        /// </summary>
        public static event Action<PlayerSpawnEventArgs> PlayerSpawnEvent;

        [SerializeField] private GameObject PlayerPref;
        public EnvironmentData EnvironmentData;

        #region Monobehaviour
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

            players = new Dictionary<string, Player>();
            IsGameRunning = false;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            Player.PlayerDeathEvent += OnPlayerDeath;
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Player.PlayerDeathEvent -= OnPlayerDeath;
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        private void Start()
        {
            if (!GamePreference.IsMultiplayer)
            {
                // Single Player, Start the game right away
                GameStart();
            }
            else
            {
                // Multiplayer, Start the game when there is enough players.
                if (PhotonNetwork.CurrentRoom.PlayerCount >= Launcher.singleton.MaxPlayerPerMatch)
                    GameStart();
                else
                    ToMainMenu();
            }
        }

        private void Update()
        {
            if (!IsGameRunning)
                return;

            if (Paused && !GamePreference.IsMultiplayer)
                return;

            LocalPlayer.AddScore(Time.deltaTime * 10);
        }

        private void OnDestroy()
        {
            Debug.Log("Cleaning up Game Scene . . . ");
            Paused = false;
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }
        #endregion

        #region Pun Callbacks
        public override void OnDisconnected(DisconnectCause cause)
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Debug.Log($"GameManager: {NetLogMessage.OTHER_LEAVE_ROOM}: {otherPlayer.NickName}");

            if (!IsGameOver)
                GameOver(PLAYER_LEFT);
        }

        #endregion

        #region Public Method
        public void ToMainMenu()
        {
            if (GamePreference.IsPlayInVR)
                StartCoroutine(VRManager.SwitchTo2D());

            if (!GamePreference.IsMultiplayer)
            {
                if (!SceneManager.GetSceneByName("MainMenu").isLoaded)
                    SceneManager.LoadSceneAsync("MainMenu");
                else
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
            }
            else
            {
                PhotonNetwork.LeaveRoom();
            }
        }
        public void GameStart()
        {
            Debug.Log("GameManager: Game Start");

            if (!GamePreference.IsMultiplayer)
                SpawnLocalPlayer(playerId: LOCAL_PLAYER_ID);
            else
                SpawnLocalPlayer(PhotonNetwork.NickName);

            IsGameRunning = true;
            Paused = false;
            if (GameStartEvent != null)
                GameStartEvent.Invoke();
        }

        public void GameOver(string reason = "")
        {
            Debug.Log($"GameManager: Game Over; Reason: {reason}");
            IsGameRunning = false;
            if (GameOverEvent != null)
                GameOverEvent.Invoke(reason);
        }

        public void Quit()
        {
            MessageBox.Show("Are you sure?", "Quit", MessageBoxButtons.YesNo, dr =>
            {
                if (dr == DialogResult.Yes)
                    Application.Quit();
            });
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Spawn a local player.
        /// </summary>
        /// <param name="playerId">The name of the player.</param>
        private void SpawnLocalPlayer(string playerId)
        {
            Debug.Log("PlayerManager: Instantiating Local Player");

            GameObject newPlayerObj = Instantiate(PlayerPref) as GameObject;

            // If is multiplayer, raise the network event.
            if (GamePreference.IsMultiplayer)
            {
                newPlayerObj.name = playerId;

                PhotonView photonView = newPlayerObj.GetComponent<PhotonView>();

                if (PhotonNetwork.AllocateViewID(photonView))
                {
                    object[] data = new object[]
                    {
                        playerId,
                        photonView.ViewID
                    };

                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                    {
                        Receivers = ReceiverGroup.Others,
                        CachingOption = EventCaching.AddToRoomCache
                    };

                    SendOptions sendOptions = new SendOptions
                    {
                        Reliability = true
                    };

                    PhotonNetwork.RaiseEvent(NetworkEventCode.InstantiatePlayer, data, raiseEventOptions, sendOptions);
                }
                else
                {
                    Debug.LogError($"PlayerManager: Failed to allocate a ViewId");

                    Destroy(newPlayerObj);
                }
            }

            Player newPlayer = newPlayerObj.GetComponent<Player>();
            LocalPlayer = newPlayer;
            RegisterPlayer(LOCAL_PLAYER_ID, newPlayer);

            PlayerAlive++;

            //Invoke Event
            if (PlayerSpawnEvent != null)
                PlayerSpawnEvent.Invoke(new PlayerSpawnEventArgs()
                {
                    isLocalPlayer = true,
                    player = newPlayer
                });
        }
        #endregion

        #region Event
        public void OnPlayerDeath(Player player)
        {
            PlayerAlive--;

            if (!GamePreference.IsMultiplayer)
            {
                // Single Player
                GameOver();
            }
            else
            {
                // Evaluate Game Over
                // If all palyers dead, game over.
                if (PlayerAlive <= 0)
                    GameOver();
                else
                {
                    foreach (Player p in players.Values)
                    {
                        if (p != player)
                        {
                            // check if the player remain has HIGHER score then the dead player.
                            if (p.Score < player.Score)
                                GameOver();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Network Event
        /// </summary>
        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == NetworkEventCode.InstantiatePlayer)
            {
                Debug.Log("PlayerManager: Spawning remote player");

                // Retrieve data
                object[] data = photonEvent.CustomData as object[];
                string playerId = (string)data[0];
                int viewId = (int)data[1];

                // Instantiate Player object
                GameObject newPlayerObj = Instantiate(PlayerPref) as GameObject;
                PhotonView view = newPlayerObj.GetComponent<PhotonView>();
                newPlayerObj.name = playerId;
                view.ViewID = viewId;

                // Register Player to dictionary
                Player newPlayer = newPlayerObj.GetComponent<Player>();
                RegisterPlayer(REMOTE_PLAYER_ID, newPlayer);

                // Invoke event
                if (PlayerSpawnEvent != null)
                    PlayerSpawnEvent.Invoke(new PlayerSpawnEventArgs()
                    {
                        isLocalPlayer = false,
                        player = newPlayer
                    });
            }
        }
        #endregion
    }
}