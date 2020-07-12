using DimensionAdventurer;
using DimensionAdventurer.Enemies;
using DimensionAdventurer.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager singleton;

        public GameObject[] Enemies;

        private Queue<GameObject> enemyQueue;

        private GameObject currentEnemy;

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

            InitQueue();
        }

        private void OnEnable()
        {
            GameManager.GameStartEvent += OnGameStart;
            GameManager.GameOverEvent += OnGameOver;
            GameManager.PauseEvent += OnGamePause;
            Portal.PortalEnteredEvent += OnPotalEntered;
        }

        private void OnDisable()
        {
            GameManager.GameStartEvent -= OnGameStart;
            GameManager.GameOverEvent -= OnGameOver;
            GameManager.PauseEvent -= OnGamePause;
            Portal.PortalEnteredEvent -= OnPotalEntered;
        }
        #endregion

        #region Public Method
        public void ActivateNextEnemy()
        {
            Debug.Log("EnemyManager: Changing enemy ");
            for (int i = 0; i < Enemies.Length; i++)
            {
                if (Enemies[i].activeInHierarchy)
                    Enemies[i].SetActive(false);
            }

            currentEnemy = GetNextEnemy();
            Debug.Log($"EnemyManager: Activating {currentEnemy.GetComponent<Enemy>().GetType().Name}");
            currentEnemy.gameObject.SetActive(true);
        }

        #endregion

        #region Private Methods
        private void InitQueue()
        {
            enemyQueue = new Queue<GameObject>();
            for (int i = 0; i < Enemies.Length; i++)
            {
                enemyQueue.Enqueue(Enemies[i]);
                Enemies[i].SetActive(false);
            }
        }

        private GameObject GetNextEnemy()
        {
            GameObject e = enemyQueue.Dequeue();
            enemyQueue.Enqueue(e);
            return e;
        }
        #endregion

        #region Event
        private void OnGameStart()
        {
            ActivateNextEnemy();
        }

        private void OnGameOver(string reason)
        {
            gameObject.SetActive(false);
        }

        private void OnGamePause(bool isPaused)
        {

        }

        private void OnPotalEntered()
        {
            ActivateNextEnemy();
        }
        #endregion
    }
}