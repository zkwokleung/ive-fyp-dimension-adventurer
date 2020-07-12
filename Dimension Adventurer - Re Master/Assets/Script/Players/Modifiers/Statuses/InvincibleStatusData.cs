using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Players.Modifiers.Statuses
{
    [CreateAssetMenu(fileName = "NewInvincibleStatus", menuName = "Status/Invincible")]
    public class InvincibleStatusData : StatusData
    {
        protected override IEnumerator IEApplyModifier(Player player, float duration)
        {
            player.IsInvincible = true;
            yield return new WaitForSeconds(duration);
            player.IsInvincible = false;
            player.UnregisterModifier(this);
        }
    }
}