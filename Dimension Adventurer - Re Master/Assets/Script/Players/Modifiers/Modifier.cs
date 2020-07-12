using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Players.Modifiers
{
    public class Modifier
    {
        public ModifierData Data { get; private set; }
        public float TimeRemain { get; private set; }

        public Modifier(ModifierData data)
        {
            Data = data;
            TimeRemain = data.duration;
        }

        public void Update()
        {
            TimeRemain -= Time.deltaTime;
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Data.Equals((obj as Modifier).Data);
        }

        public static bool operator==(Modifier m1, Modifier m2)
        {
            return m1.Equals(m2);
        }

        public static bool operator!=(Modifier m1, Modifier m2)
        {
            return !m1.Equals(m2);
        }
    }
}