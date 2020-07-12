using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DimensionAdventurer.UI
{
    public class InGameUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject PauseMenu;
        [SerializeField] private GameObject GameOverMenu;
        [SerializeField] private TextMeshProUGUI[] PostGameMessages;

        private void OnEnable()
        {
            GameManager.PauseEvent += OnPause;
            GameManager.GameOverEvent += OnGameOver;
        }

        private void OnDisable()
        {
            GameManager.PauseEvent -= OnPause;
            GameManager.GameOverEvent -= OnGameOver;
        }

        #region Event
        private void OnPause(bool IsPause)
        {
            PauseMenu.SetActive(IsPause);
        }

        private void OnGameOver(string reason)
        {
            if (PostGameMessages != null)
                foreach (TextMeshProUGUI tmp in PostGameMessages)
                {
                    if (reason != string.Empty)
                        tmp.text = reason;
                }

            GameOverMenu.SetActive(true);
        }
        #endregion

        #region UI Control
        public void OnBtnPausePress()
        {
            GameManager.Paused = true;
        }
        #endregion
    }
}