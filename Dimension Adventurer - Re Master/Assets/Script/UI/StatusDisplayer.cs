using UnityEngine;
using UnityEngine.UI;
using DimensionAdventurer.Players.Modifiers.Statuses;
using DimensionAdventurer.Players;
using DimensionAdventurer.Players.Modifiers;

namespace DimensionAdventurer.UI
{
    public class StatusDisplayer : MonoBehaviour
    {
        //Status Data
        private Modifier modifier { get; set; }

        //UI element
        [SerializeField] private Image imgIcon;
        [SerializeField] private Image imgTimer;

        public void Initialize(Modifier modifier)
        {
            this.modifier = modifier;
            imgIcon.sprite = ((StatusData)this.modifier.Data).icon;
            imgTimer.fillAmount = 1;
        }

        void Update()
        {
            modifier.Update();
            imgTimer.fillAmount = (modifier.TimeRemain / modifier.Data.duration);
            if (imgTimer.fillAmount <= 0)
                gameObject.SetActive(false);
        }

        private void OnDestroy()
        {

        }
    }
}