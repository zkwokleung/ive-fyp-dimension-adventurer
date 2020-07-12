using DimensionAdventurer.Networking;
using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DimensionAdventurer.UI.Multiplayers
{
    public class MultiplayerMenu : MonoBehaviourPunCallbacks
    {
        public enum Panel
        {
            Connect,
            MatchSelect
        }

        public static MultiplayerMenu singleton { get; private set; }

        [SerializeField] private GameObject pnlConnect;
        [SerializeField] private GameObject pnlMatchSelect;

        [SerializeField] private TMP_InputField ifNickname;
        [SerializeField] private TMP_InputField ifPassword;

        private string nickname;
        private string password;

        private event Action btnBackClickEvent;

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
            btnBackClickEvent = () => MainMenu.singleton.SwitchPanel(MainMenu.Panel.GamemodeSelect);
        }

        #region Pun Callback
        public override void OnConnectedToMaster()
        {
            SwitchPanel(Panel.MatchSelect);
            btnBackClickEvent = () => PhotonNetwork.Disconnect();
        }

        public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
        {
            SwitchPanel(Panel.Connect);
            btnBackClickEvent = () => MainMenu.singleton.SwitchPanel(MainMenu.Panel.GamemodeSelect);
        }

        public override void OnLeftRoom()
        {
            SwitchPanel(Panel.MatchSelect);
        }
        #endregion

        #region Button Click 
        public void OnBtnConnectClick()
        {
            nickname = ifNickname.text;
            Launcher.singleton.Connect(nickname);
        }

        public void OnBtnQuickMatchClick()
        {
            Launcher.singleton.JoinRandomMatch();
        }

        public void OnBtnPwMatchClick()
        {
            password = ifPassword.text;
            Launcher.singleton.JoinPasswordMatch(password);
        }

        public void OnBtnBackClick()
        {
            if (btnBackClickEvent != null)
                btnBackClickEvent.Invoke();
        }
        #endregion

        public void SwitchPanel(Panel panel)
        {
            pnlConnect.SetActive(false);
            pnlMatchSelect.SetActive(false);

            switch (panel)
            {
                case Panel.Connect:
                    pnlConnect.SetActive(true);
                    break;

                case Panel.MatchSelect:
                    pnlMatchSelect.SetActive(true);
                    break;
            }
        }
    }
}