using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DimensionAdventurer.Players;

namespace DimensionAdventurer.UI
{
    public class UIPlayerInfo : MonoBehaviour
    {
        private Player Player;

        #region UI Element
        [SerializeField] private GameObject PauseButton;
        [SerializeField] private Slider sldHealth;
        [SerializeField] private TextMeshProUGUI txtHealth;
        [SerializeField] private Slider sldCharge;
        [SerializeField] private TextMeshProUGUI txtScore;
        [SerializeField] private GameObject DebugText;
        #endregion

        private void OnEnable()
        {
            GameManager.PlayerSpawnEvent += OnPlayerSpawn;
        }

        private void OnDisable()
        {
            GameManager.PlayerSpawnEvent -= OnPlayerSpawn;
        }

        void Start()
        {
#if UNITY_ANDROID || UNITY_EDITOR
            PauseButton.SetActive(true);
#elif UNITY_STANDALONE
            PauseButton.SetActive(false);
#endif
        }

        void Update()
        {
            if (Player != null)
                UpdateData();


#if UNITY_STANDALONE || UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F2))
                DebugText.SetActive(!DebugText.activeInHierarchy);
#endif
        }

        private void UpdateData()
        {
            //HP
            txtHealth.text = string.Format("HP: {0}/{1}", (object)Player.Health, (object)Player.MaxHealth);
            sldHealth.value = Player.Health;

            //Charge
            sldCharge.value = Player.Charge;

            //Score
            txtScore.text = Player.Score.ToString("0");
        }

        public void OnPlayerSpawn(PlayerSpawnEventArgs e)
        {
            if (e.isLocalPlayer)
                Player = e.player;

            sldHealth.maxValue = Player.MaxHealth;
        }
    }
}