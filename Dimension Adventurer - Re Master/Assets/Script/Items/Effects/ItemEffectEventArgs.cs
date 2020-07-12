using DimensionAdventurer.Players;
using System;

namespace DimensionAdventurer.Items.Effects
{
    public class ItemEffectEventArgs : EventArgs
    {
        public object Producer;
        public Player player;
    }
}