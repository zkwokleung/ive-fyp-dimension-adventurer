using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DimensionAdventurer.Items.Effects
{
    /// <summary>
    /// The effect of adding charges to a player
    /// </summary
    [CreateAssetMenu(fileName = "NewChargeEffect", menuName = "Effects/Charge")]
    public class ChargeEffect : ItemEffect
    {
        public int amount = 1;

        public override void ExecuteEffect(GameObject source, ItemEffectEventArgs e)
        {
            base.ExecuteEffect(source, e);
            e.player.Charge += amount;
        }
    }

}