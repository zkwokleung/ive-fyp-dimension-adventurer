using DimensionAdventurer.Players.Modifiers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Players
{
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField] private Player player;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip acHeal;
        [SerializeField] private AudioClip acDamage;
        [SerializeField] private AudioClip acDead;
        [SerializeField] private AudioClip acModifier;
        [SerializeField] private AudioClip acItemPickUp;

        private void OnEnable()
        {
            player.HealEvent += OnPlayerHeal;
            player.DamageEvent += OnPlayerDamaged;
            player.DeathEvent += OnPlayerDead;
            player.ModifierEvent += OnModifierApply;
            player.ItemPickUpEvent += OnItemPickUp;
        }

        private void OnDisable()
        {
            player.HealEvent -= OnPlayerHeal;
            player.DamageEvent -= OnPlayerDamaged;
            player.DeathEvent -= OnPlayerDead;
            player.ModifierEvent -= OnModifierApply;
            player.ItemPickUpEvent -= OnItemPickUp;
        }

        #region Private Methods
        private void PlayClip(AudioClip ac)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.PlayOneShot(ac);
        }
        #endregion

        #region Event
        private void OnPlayerHeal()
        {
            if (acHeal != null)
                PlayClip(acHeal);
        }

        private void OnPlayerDamaged()
        {
            if(acDamage != null)
                PlayClip(acDamage);
        }

        private void OnPlayerDead()
        {
            if (acDead != null)
                PlayClip(acDead);
        }

        private void OnModifierApply(object source, ModifierEventArgs e)
        {
            if (acModifier != null)
                PlayClip(acModifier);
        }

        private void OnItemPickUp(GameObject Item)
        {
            // Disabled. Too Annoying 
            //if (acItemPickUp != null)
            //    PlayClip(acItemPickUp);
        }
        #endregion
    }
}