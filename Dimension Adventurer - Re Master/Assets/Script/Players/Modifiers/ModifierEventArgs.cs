using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Players.Modifiers
{
    [Serializable]
    public class ModifierEventArgs : EventArgs
    {
        public enum Action
        {
            Add,
            Remove,
            Suspend,
            Reinstate
        }

        public object Provider;
        public Modifier Modifier;
        public Player Player;
        public Action action;
    }

    
}