using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

namespace DimensionAdventurer.UI
{
    public class OptionMenu : MonoBehaviour
    {
        private static Vector2Int[] RESOLUTIONS =
        {
            new Vector2Int(800, 450),
            new Vector2Int(960, 540),
            new Vector2Int(1280, 720),
            new Vector2Int(1366, 768),
            new Vector2Int(1600,900),
            new Vector2Int(1920, 1080)
        };
        public static event Action<Vector2Int> ResolutionChangeEvent;
        public static event Action<bool> FullScreenChangeEvent;
        public static event Action<float> VolumeChangeEvent;

        [SerializeField] private GameObject pnlResolution;

        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private TMP_Dropdown ddResolution;

        private void OnEnable()
        {
            InitDropdownResolution();
        }

        #region Private Methods
        private void InitDropdownResolution()
        {
            // Disable set window size in mobile
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                pnlResolution.SetActive(false);
                return;
            }
            int currentResolutionIndex = 0;
            for (int i = 0; i < RESOLUTIONS.Length; i++)
            {
                if (RESOLUTIONS[i].x == Screen.width && RESOLUTIONS[i].y == Screen.height)
                    currentResolutionIndex = i;
            }
            ddResolution.value = currentResolutionIndex;
        }
        #endregion

        #region Public Methods
        public void SetVolume(float volume)
        {
            audioMixer.SetFloat("volume", volume);
        }

        public void SetResolution(int resolutionIndex)
        {
            Screen.SetResolution(RESOLUTIONS[resolutionIndex].x, RESOLUTIONS[resolutionIndex].y, Screen.fullScreen);
        }

        public void SetFullScreen(bool fullscreen)
        {
            Screen.fullScreen = fullscreen;
        }
        #endregion
    }
}