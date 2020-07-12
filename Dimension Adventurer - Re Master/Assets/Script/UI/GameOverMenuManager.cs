using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.UI
{
    public class GameOverMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject singlePlayerMenu;
        [SerializeField] private GameObject multiplayerMenu;

        private void OnEnable()
        {
            if (!GamePreference.IsMultiplayer)
                singlePlayerMenu.SetActive(true);
            else
                multiplayerMenu.SetActive(true);
        }
    }
}