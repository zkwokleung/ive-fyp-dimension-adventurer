using System.Collections;
using UnityEngine;

namespace DimensionAdventurer.Players.Modifiers.Statuses
{
    public class StatusData : ModifierData
    {
        /// <summary>
        /// The icon of the status
        /// </summary>
        public Sprite icon;


        public override void Apply(Player player)
        {
            base.Apply(player);   
        }

        public override void OnModifierInvoke()
        {
            base.OnModifierInvoke();
        }

        protected override IEnumerator IEApplyModifier(Player player, float duration)
        {
            yield return null;
        }
    }
}