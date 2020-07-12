using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Players.Modifiers
{
    public abstract class ModifierData : ScriptableObject
    {
        public new string name;
        public float duration =7f;

        [Multiline]
        public string description;

        public virtual void Apply(Player player)
        {
            OnModifierInvoke();
            player.RegisterModifier(this);
            CoroutineHandler.StartCoroutine(name, IEApplyModifier(player, duration));
        }

        public virtual void OnModifierInvoke()
        {

        }

        protected abstract IEnumerator IEApplyModifier(Player player, float duration);

        public override bool Equals(object other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public static bool operator==(ModifierData m1, ModifierData m2)
        {
            return m1.Equals(m2);
        }

        public static bool operator!=(ModifierData m1, ModifierData m2)
        {
            return !m1.Equals(m2);
        }
    }
}