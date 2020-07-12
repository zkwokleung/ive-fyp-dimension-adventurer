using DimensionAdventurer.Players;
using DimensionAdventurer.Items.Effects;
using UnityEngine;

namespace DimensionAdventurer.Items
{
    public interface ICollectable
    {
        void OnCollected(GameObject source, CollectEventArgs e);
    }

    public class CollectEventArgs
    {
        public Player player;
    }
}