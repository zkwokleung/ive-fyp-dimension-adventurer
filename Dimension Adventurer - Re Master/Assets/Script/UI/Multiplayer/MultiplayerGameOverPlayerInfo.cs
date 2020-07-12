using DimensionAdventurer.Players;
using TMPro;
using UnityEngine;

namespace DimensionAdventurer.UI.Multiplayers
{
    public class MultiplayerGameOverPlayerInfo : MonoBehaviour
    {
        private Player player;
        public Player Player
        {
            get => player;
            set
            {
                player = value;
                txtName.text = player.gameObject.name;

                txtScore.text = Player.Score.ToString("0");
                txtItem.text = Player.Collected.ToString();
            }
        }
        [SerializeField] private TextMeshProUGUI txtName;
        [SerializeField] private TextMeshProUGUI txtScore;
        [SerializeField] private TextMeshProUGUI txtItem;
    }
}