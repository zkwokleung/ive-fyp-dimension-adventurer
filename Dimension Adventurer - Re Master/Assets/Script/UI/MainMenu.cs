using DimensionAdventurer.UI;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DimensionAdventurer
{

    public class MainMenu : MonoBehaviourPunCallbacks
    {
        public enum Panel
        {
            Welcome,
            GamemodeSelect,
            CharacterSelect,
            Multiplayer,
            Option
        }

        #region Static
        public static MainMenu singleton { get; private set; }
        #endregion

        public Panel CurrentPanel { get; private set; }

        /// <summary>
        /// A lsit that contains all the panels for easier management.
        /// </summary>
        private List<GameObject> _panels;
        [SerializeField] private GameObject pnlWelcome;
        [SerializeField] private GameObject pnlGamemode;
        [SerializeField] private GameObject pnlMultiplayer;

        [SerializeField] private Toggle cbVR;

        #region MonoBehaviour
        private void Awake()
        {
            // Singleton
            if (singleton == null)
                singleton = this;
            else if (singleton != this)
            {
                Debug.Log("Instance already exists, destroying object . . . ");
                Destroy(this);
            }

            Cursor.visible = true;
            InitializePanelList();
            Screen.orientation = ScreenOrientation.Landscape;
        }

        private void Start()
        {
            SwitchPanel(Panel.Welcome);
        }

        private void Update()
        {
            //Press to start
            if (pnlWelcome.activeInHierarchy && (Input.anyKey || Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
                SwitchPanel(Panel.GamemodeSelect);
        }

        private void OnDestroy()
        {
            Debug.Log("Cleaning up main menu . . . ");
            SceneManager.UnloadSceneAsync("MainMenu");
        }
        #endregion

        #region PunCallbacks
        public override void OnConnected()
        {
            GamePreference.singleton.isMultiplayer = true;
        }

        public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
        {
            GamePreference.singleton.isMultiplayer = false;
        }
        #endregion

        private void InitializePanelList()
        {
            _panels = new List<GameObject>();
            _panels.Add(pnlWelcome);
            _panels.Add(pnlGamemode);
            _panels.Add(pnlMultiplayer);
            DisablePanels();
        }

        public void OnBtnSinglePlayerClick()
        {
            GamePreference.singleton.isPlayInVR = cbVR.isOn;
            GamePreference.singleton.isMultiplayer = false;
            SceneManager.LoadSceneAsync("GameScene");
        }

        public void OnBtnMultiplayerClick()
        {
            SwitchPanel(Panel.Multiplayer);
        }

        public void OnBtnQuitClick()
        {
            Application.Quit();
        }

        public void SwitchPanel(Panel panel)
        {
            DisablePanels();
            switch (panel)
            {
                case Panel.Welcome:
                    pnlWelcome.SetActive(true);
                    break;

                case Panel.GamemodeSelect:
                    pnlGamemode.SetActive(true);
                    break;

                case Panel.Multiplayer:
                    pnlMultiplayer.SetActive(true);
                    break;

                default:
                    SimpleTextMessage.Show(this, "Option unavailable yet");
                    SwitchPanel(CurrentPanel);
                    break;
            }
            //Set to displayed panel
            CurrentPanel = panel;
        }

        private void DisablePanels()
        {
            foreach (GameObject pnl in _panels)
                pnl.SetActive(false);
        }
    }
}