using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DimensionAdventurer.UI
{
    public class UIEffect : MonoBehaviour
    {
        [SerializeField] private Image effectFrame;

        /// <summary>
        /// For controlling the coroutine
        /// </summary>
        private IEnumerator _showEffect;

        private void OnEnable()
        {
            GameManager.PlayerSpawnEvent += OnPlayerSpawn;
        }

        private void OnDisable()
        {
            GameManager.PlayerSpawnEvent -= OnPlayerSpawn;
        }

        #region Event
        public void OnPlayerHeal()
        {
            ShowHeallingEffect(0.5f);
        }

        public void OnPlayerDamaged()
        {
            ShowInjureEffect(0.5f);
        }

        public void OnPlayerDead()
        {

        }

        public void OnPlayerSpawn(PlayerSpawnEventArgs e)
        {
            if(e.isLocalPlayer)
            {
                //Registering Events
                e.player.HealEvent += OnPlayerHeal;
                e.player.DamageEvent += OnPlayerDamaged;
                e.player.DeathEvent += OnPlayerDead;
            }
        }
        #endregion


        #region Camera Effect
        public void ShowInjureEffect(float duration)
        {
            effectFrame.color = new Color(255, 0, 0, 0);
            ShowEffect(duration);
        }

        public void ShowInvincibleEffect(float duration)
        {
            effectFrame.color = new Color(255, 255, 255, 0);
            ShowEffect(duration);
        }

        public void ShowHeallingEffect(float duration)
        {
            effectFrame.color = Color.green;
            ShowEffect(duration);
        }

        private void ShowEffect(float duration)
        {
            if (_showEffect != null)
                StopCoroutine(_showEffect);

            _showEffect = ShowFrame(duration);
            StartCoroutine(_showEffect);
        }
        
        private IEnumerator ShowFrame(float duration)
        {
            Color c = effectFrame.color;
            c.a = 1f;
            effectFrame.color = c;
            yield return new WaitForSeconds(duration);
            while(effectFrame.color.a > 0)
            {
                c = effectFrame.color;
                c.a -= 0.10f;
                effectFrame.color = c;
                yield return new WaitForSeconds(0.05f);
            }
        }

        #endregion
    }
}
