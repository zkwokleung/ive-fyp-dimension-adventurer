using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DimensionAdventurer
{
    public class GamePreference : MonoBehaviour
    {
        public static GamePreference singleton { get; private set; }
        public static bool IsPlayInVR { get => singleton.isPlayInVR; }
        public static bool IsMultiplayer { get => singleton.isMultiplayer; }

        public bool isPlayInVR = false;
        public bool isMultiplayer = false;

        private void Awake()
        {
            if (singleton == null)
                singleton = this;
            else if (singleton != this)
            {
                Debug.Log($"{GetType().Name}: Instance already exists, destroying object . . . ");
                Destroy(gameObject);
            }

            Reset();
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
        }

        public void Reset()
        {
            isPlayInVR = false;
            isMultiplayer = false;
        }

        private void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "MainMenu")
            {
                Reset();
            }
        }
    }
}