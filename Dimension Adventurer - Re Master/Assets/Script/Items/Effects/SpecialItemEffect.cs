using DimensionAdventurer.Players;
using DimensionAdventurer.Players.Modifiers;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Items.Effects
{
    [CreateAssetMenu(fileName = "NewSpecialItemEffect", menuName = "Effects/Speicl item effect")]
    public class SpecialItemEffect : ItemEffect
    {
        public List<ModifierData> modifiers;

        public override void ExecuteEffect(GameObject source, ItemEffectEventArgs e)
        {
            base.ExecuteEffect(source, e);

            foreach(ModifierData m in modifiers)
                m.Apply(e.player);
        }
    }
}