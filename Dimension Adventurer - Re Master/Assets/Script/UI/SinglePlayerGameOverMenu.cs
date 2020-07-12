using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DimensionAdventurer.Players;
using DimensionAdventurer.Storages;

namespace DimensionAdventurer.UI
{
    public class SinglePlayerGameOverMenu : MonoBehaviour
    {
        private Player player;
        [SerializeField] private TextMeshProUGUI txtHighScore;
        [SerializeField] private TextMeshProUGUI txtScore;
        [SerializeField] private TextMeshProUGUI txtItem;

        private void OnEnable()
        {
            this.player = GameManager.LocalPlayer;
            txtHighScore.text = StorageManager.singleton.highScoreHistory.ToString("0");
            txtScore.text = player.Score.ToString("0");
            txtItem.text = player.Collected.ToString();
        }
    }
}