using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Items.Effects
{
    [CreateAssetMenu(fileName = "New Healing Effect", menuName = "Effects/Healing")]
    public class HealEffect : ItemEffect
    {
        public float amount = 1;

        public override void ExecuteEffect(GameObject source, ItemEffectEventArgs e)
        {
            base.ExecuteEffect(source, e);

            e.player.Heal(amount);
        }
    }
}