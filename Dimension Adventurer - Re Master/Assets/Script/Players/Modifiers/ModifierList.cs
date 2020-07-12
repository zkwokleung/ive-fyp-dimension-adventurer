using System;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Players.Modifiers
{
    public class ModifierList : MonoBehaviour
    {
        public Player Player { get; private set; }
        public List<Modifier> Modifiers { get; private set; }
        public int Count { get => Modifiers.Count; }

        public event Action<object, ModifierEventArgs> ModifierEvent;

        #region MonoBehaviour
        private void Awake()
        {
            Player = gameObject.GetComponent<Player>();
            Modifiers = new List<Modifier>();
        }

        private void Update()
        {
            for(int i = 0; i < Modifiers.Count; i++)
            {
                Modifiers[i].Update();
                if (Modifiers[i].TimeRemain <= 0)
                    Unregister(Modifiers[i]);
            }
        }
        #endregion

        #region Modifiers
        public void Register(ModifierData data)
        {
            Modifier newMod = new Modifier(data);
            Modifiers.Add(new Modifier(data));
            OnModifierRegister(newMod);
        }

        public void Unregister(ModifierData data)
        {
            Modifier mod;
            if (ContainModifier(data, out mod))
                Unregister(mod);
        }

        public void Unregister(Modifier modifier)
        {
            Modifiers.Remove(modifier);
            OnModifierUnregister(modifier);
        }

        private void Unregister(int index)
        {
            Modifiers.RemoveAt(index);
        }

        public bool ContainModifier(ModifierData data, out Modifier modifier)
        {
            foreach(Modifier m in Modifiers)
            {
                if (m.Data .Equals(data))
                {
                    modifier = m;
                    return true;
                }
            }
            modifier = null;
            return false;
        }

        public bool ContainModifier(ModifierData data, out ModifierData modifier)
        {
            foreach (Modifier m in Modifiers)
            {
                if (m.Data.Equals(data))
                {
                    modifier = m.Data;
                    return true;
                }
            }
            modifier = null;
            return false;
        }
        #endregion

        #region Event
        private void OnModifierRegister(Modifier modifier)
        {
            if(ModifierEvent != null)
                ModifierEvent.Invoke(this, new ModifierEventArgs()
                {
                    Player = this.Player,
                    Modifier = modifier,
                    action = ModifierEventArgs.Action.Add
                });
        }

        private void OnModifierUnregister(Modifier modifier)
        {
            if (ModifierEvent != null)
                ModifierEvent.Invoke(this, new ModifierEventArgs()
                {
                    Player = this.Player,
                    Modifier = modifier,
                    action = ModifierEventArgs.Action.Remove
                });
        }
        #endregion
    }
}