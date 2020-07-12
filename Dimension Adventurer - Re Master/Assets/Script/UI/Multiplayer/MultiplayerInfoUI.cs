using UnityEngine;

namespace DimensionAdventurer.UI.Multiplayers
{
    public class MultiplayerInfoUI : MonoBehaviour
    {
        [Header("Single player UI")]
        [SerializeField] private GameObject hpBar;

        [Header("Multiplayer UI")]
        [SerializeField] private GameObject scoreBoard;

        #region MonoBehaviour
        private void Start()
        {
            if (!GamePreference.IsMultiplayer)
            {
                // Single Player
                scoreBoard.SetActive(false);
            }
            else
            {
                // Multiplayer
                scoreBoard.SetActive(true);
                hpBar.SetActive(false);
            }
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
