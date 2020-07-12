using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Inputs
{
    public class InputEventArgs : EventArgs
    {
        public enum Performed
        {
            Left,
            Right,
            Up,
            Down,
        }

        public Performed performed;
    }
}