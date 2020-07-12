using DimensionAdventurer.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Storages
{
    public class StorageManager : MonoBehaviour
    {
        public static StorageManager singleton { get; private set; }
        private const string HIGH_SCORE_KEY = "highScore";
        public float highScoreHistory = 0;
        private Player player;

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
            if (PlayerPrefs.HasKey(HIGH_SCORE_KEY))
                highScoreHistory = PlayerPrefs.GetFloat(HIGH_SCORE_KEY);

            GameManager.GameStartEvent += OnGameStart;
        }

        private void OnDestroy()
        {
            GameManager.GameStartEvent -= OnGameStart;

            if (player != null)
                player.DeathEvent -= OnPlayerDead;
        }

        private void OnGameStart()
        {
            player = GameManager.LocalPlayer;
            player.DeathEvent += OnPlayerDead;
        }

        private void OnPlayerDead()
        {
            // Save the score to the local storage
            if (player.Score > highScoreHistory)
            {
                PlayerPrefs.SetFloat(HIGH_SCORE_KEY, player.Score);
                PlayerPrefs.Save();
            }
        }
    }
}