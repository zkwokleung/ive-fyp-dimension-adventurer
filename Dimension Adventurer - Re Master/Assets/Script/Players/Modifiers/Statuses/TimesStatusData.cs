using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Players.Modifiers.Statuses
{
    [CreateAssetMenu(fileName = "NewTimesStatus", menuName = "Status/Times")]
    public class TimesStatusData : StatusData
    {
        public int magnitude;

        public override void Apply(Player player)
        {
            //Do not apply TImes status if the player already have a better one.
            ModifierData other;
            if(player.Modifiers.ContainModifier(this, out other)){
                if ((other as TimesStatusData).magnitude > this.magnitude)
                    return;
            }

            OnModifierInvoke();
            player.RegisterModifier(this);
            CoroutineHandler.StartCoroutine("Times", IEApplyModifier(player, duration));
        }

        protected override IEnumerator IEApplyModifier(Player player, float duration)
        {
            player.ScoreMultiplier = magnitude;
            yield return new WaitForSeconds(duration);
            player.ScoreMultiplier = 1;
            player.UnregisterModifier(this);
        }

        public override int GetHashCode()
        {
            return "Times".GetHashCode();
        }

        public override bool Equals(object other)
        {
            return GetHashCode().Equals(other.GetHashCode());
        }
    }
}