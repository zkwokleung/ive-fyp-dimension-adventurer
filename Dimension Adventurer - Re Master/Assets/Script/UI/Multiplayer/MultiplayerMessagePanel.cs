using DimensionAdventurer.Networking;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DimensionAdventurer.UI.Multiplayers
{
    public class MultiplayerMessagePanel : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TextMeshProUGUI txtMessage;

        private event Action BtnCancelClickEvent;

        #region MonoBehaviour
        private void Start()
        {
            Launcher.LogFeedbackEvent += OnLogFeedback;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Launcher.LogFeedbackEvent -= OnLogFeedback;
        }
        #endregion

        #region PunCallback
        public override void OnConnectedToMaster()
        {
            BtnCancelClickEvent = () =>PhotonNetwork.Disconnect();
            gameObject.SetActive(false);
        }

        public override void OnJoinedRoom()
        {
            BtnCancelClickEvent = () => PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            BtnCancelClickEvent = () => PhotonNetwork.Disconnect();
            gameObject.SetActive(false);
        }
        #endregion

        #region Event
        private void OnLogFeedback(string message)
        {
            txtMessage.text = message;
            gameObject.SetActive(true);
        }
        #endregion

        #region Button Click
        public void OnBtnCancelClicked()
        {
            gameObject.SetActive(false);
            if (BtnCancelClickEvent != null)
                BtnCancelClickEvent.Invoke();
        }
        #endregion
    }
}