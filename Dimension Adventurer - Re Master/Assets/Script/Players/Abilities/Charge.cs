using DimensionAdventurer.Inputs;
using DimensionAdventurer.Players.Modifiers;
using System;
using UnityEngine;

namespace DimensionAdventurer.Players.Abilities
{
    public class Charge   : MonoBehaviour
    {
        [SerializeField] private Player player;
        /// <summary>
        /// Modifiers that would apply when the charge is activated.
        /// </summary>
        [SerializeField] private ModifierData[] modifiers;

        private void OnEnable()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                MobileInputEventSystem.MobileInputEvent += OnMobileInput;
            StandaloneInputEventSystem.ChargeButtonClickedEvent += OnChargeButtonClicked;
        }

        private void OnDisable()
        {
            StandaloneInputEventSystem.ChargeButtonClickedEvent -= OnChargeButtonClicked;
        }

        public void OnChargeButtonClicked()
        {
            ChargeUp();
        }

        public void OnMobileInput(MobileInputEventArgs e)
        {
            if (e.gesture == Gesture.DoubleTap)
                ChargeUp();
        }

        public void ChargeUp()
        {
            // Not enought charges -> do nothing
            if (player.Charge < 3)
            {
                return;
            }

            // Apply the modifiers event to the player
            foreach (ModifierData m in modifiers)
            {
                m.Apply(player);
            }
            // Remove all the charges
            player.Charge = 0;
        }
    }
}