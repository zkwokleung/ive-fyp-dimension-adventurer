using DimensionAdventurer.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Enemies
{
    /// <summary>
    /// Handling attackss from enemies. Each player must attach one of this.
    /// </summary>
    public class AttackListener : MonoBehaviour
    {
        [SerializeField] private Player player;

        #region MonoBehaviour
        private void OnEnable()
        {
            AttackManager.singleton.RegisterListener(this);
        }

        private void OnDisable()
        {
            AttackManager.singleton.UnregisterListener(this);
        }
        #endregion



        /// <summary>
        /// Function called when a enemy raise an attack.
        /// </summary>
        public void OnAttackRaise(WorldPosition position, float damage)
        {
            if (IsPlayerHitted(position))
                player.Damage(damage);
        }

        /// <summary>
        /// Check if the player is standing on where the enemy is attacking.
        /// </summary>
        private bool IsPlayerHitted(WorldPosition position)
        {
            return player.WorldPosition == position;
        }
    }
}