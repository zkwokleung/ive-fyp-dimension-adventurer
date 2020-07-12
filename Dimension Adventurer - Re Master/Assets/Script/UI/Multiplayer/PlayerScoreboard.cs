using DimensionAdventurer.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DimensionAdventurer.UI.Multiplayers
{
    public class PlayerScoreboard : MonoBehaviour
    {
        private Player player;
        public Player Player
        {
            get => player;
            set
            {
                player = value;
                txtName.text = player.name;
                txtScore.text = "0";
                sldHp.maxValue = player.MaxHealth;
                sldHp.value = player.Health;
            }
        }
        [SerializeField] private TextMeshProUGUI txtName;
        [SerializeField] private TextMeshProUGUI txtScore;
        [SerializeField] private Slider sldHp;

        void Update()
        {
            if (player == null)
                return;

            sldHp.value = player.Health;
            txtScore.text = player.Score.ToString("0");
        }
    }
}