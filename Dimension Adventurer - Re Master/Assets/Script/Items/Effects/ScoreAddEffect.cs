using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Items.Effects
{
    /// <summary>
    /// The effect of adding scores to a player.
    /// </summary
    [CreateAssetMenu(fileName = "NewAddScoreEffect", menuName = "Effects/Add Score")]
    public class ScoreAddEffect : ItemEffect
    {
        public float amount = 100;

        public override void ExecuteEffect(GameObject source, ItemEffectEventArgs e)
        {
            base.ExecuteEffect(source, e);
            e.player.AddScore(amount);
        }
    }
}