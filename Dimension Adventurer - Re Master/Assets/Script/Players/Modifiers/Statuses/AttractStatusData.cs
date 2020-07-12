using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Players.Modifiers.Statuses
{
    [CreateAssetMenu(fileName = "NewAttractStatus", menuName = "Status/Attract")]
    public class AttractStatusData : StatusData
    {
        protected override IEnumerator IEApplyModifier(Player player, float duration)
        {
            GameObject ic = ObjectPool.singleton.Spawn("Item Collector", player.gameObject.transform.localPosition);
            ic.GetComponent<ItemCollector>().OnCreated(player);
            yield return new WaitForSeconds(duration);
            ic.SetActive(false);
            player.UnregisterModifier(this);
        }
    }
}