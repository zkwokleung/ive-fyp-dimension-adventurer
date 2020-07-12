using DimensionAdventurer.Players;
using System;

namespace DimensionAdventurer
{
    public class PlayerSpawnEventArgs : EventArgs
    {
        public Player player;
        public bool isLocalPlayer;
    }
}