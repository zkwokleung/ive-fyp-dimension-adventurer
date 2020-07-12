using System;
using UnityEngine;
using DimensionAdventurer.Math;
using DimensionAdventurer.Items;
using Random = UnityEngine.Random;
using DimensionAdventurer.Players.Modifiers;

namespace DimensionAdventurer.Players
{
    public class Player : MonoBehaviour
    {
        #region Static
        public static float DEFAULT_MAX_HEALTH = 6;

        public static event Action<Player> PlayerDamagedEvent;
        public static event Action<Player> PlayerDeathEvent;
        #endregion

        private float health = DEFAULT_MAX_HEALTH;
        /// <summary>
        /// The hp of the play. Can be reduced during runtime.
        /// </summary>
        public float Health
        {
            get => health;
            set {
                health = value;
                if (health <= 0)
                {
                    health = 0;
                    Dead();
                }
            }
        }

        /// <summary>
        /// Is the player considered dead.
        /// </summary>
        public bool IsDead { get; private set; }

        /// <summary>
        /// The score of the player.
        /// </summary>
        public float Score;

        /// <summary>
        /// The maximum health of player.
        /// </summary>
        public float MaxHealth;

        /// <summary>
        /// The immunity status of player.
        /// </summary>
        public bool IsInvincible;

        /// <summary>
        /// The Score Multiplier of the player.
        /// </summary>
        public float ScoreMultiplier = 1;

        /// <summary>
        /// Items that this player has collected.
        /// </summary>
        public int Collected = 0;

        /// <summary>
        /// The charge up amount of the player.
        /// </summary>
        public int Charge
        {
            get => charge;
            set
            {
                charge = value;

                if (charge > 3)
                    charge = 3;
                else if (charge < 0)
                    charge = 0;
            }
        }
        private int charge;

        public WorldPosition WorldPosition;

        /// <summary>
        /// The list of item effects applied on the player.
        /// </summary>
        public ModifierList Modifiers { get; private set; }

        #region Events
        // Item pick up
        public event Action<GameObject> ItemPickUpEvent;

        // Heal
        public event Action HealEvent;

        // Damaged
        public event Action DamageEvent;

        // Death
        public event Action DeathEvent;

        // Modifier Event
        public event Action<object, ModifierEventArgs> ModifierEvent
        {
            add => Modifiers.ModifierEvent += value;
            remove => Modifiers.ModifierEvent -= value;
        }

        #endregion

        #region Public Methods
        public void RegisterModifier(ModifierData modifier)
        {
            Modifiers.Register(modifier);
        }

        public void UnregisterModifier(ModifierData modifier)
        {
            Modifiers.Unregister(modifier);
        }

        public void AddScore(float amount)
        {
            MathHelper.PreventNegative(amount);

            Score += amount * ScoreMultiplier;
        }

        public void DeductScore(float amount)
        {
            MathHelper.PreventNegative(amount);

            Score -= amount;
        }

        public void Heal(float amount)
        {
            MathHelper.PreventNegative(amount);

            //Prevent overheal
            if (Health + amount > MaxHealth)
            {
                Health = MaxHealth;
            }
            else
            {
                Health += amount;
            }

            if (HealEvent != null)
                HealEvent.Invoke();
        }

        public void Damage(float amount)
        {
            MathHelper.PreventNegative(amount);

            if (IsInvincible)
            {
                //Debug.Log("Attack Blocked");
                return;
            }

            Health -= amount;
            //Debug.Log("Dealing " + amount + " damage(s) to the player.");
            if (DamageEvent != null)
                DamageEvent.Invoke();
            if (PlayerDamagedEvent != null)
                PlayerDamagedEvent.Invoke(this);
        }

        private void Dead()
        {
            IsDead = true;

            if (DeathEvent != null)
                DeathEvent.Invoke();
            if (PlayerDeathEvent != null)
                PlayerDeathEvent.Invoke(this);
        }
        #endregion

        #region MonoBehaviour
        private void Awake()
        {
            IsInvincible = false;
            IsDead = false;
            MaxHealth = DEFAULT_MAX_HEALTH;
            Health = MaxHealth;
            Score = 0f;
            Charge = 0;
            Collected = 0;
            Modifiers = GetComponent<ModifierList>();
            WorldPosition = new WorldPosition(PlaneType.Floor, RunningTrack.Middle);
        }

        private void OnTriggerEnter(Collider other)
        {
            //When the gameobject collides with other objects
            if (other.CompareTag("Item"))
            {
                //Check if it is a collectable 
                ICollectable ic = other.GetComponent<ICollectable>();
                if (ic != null)
                    ic.OnCollected(gameObject, new CollectEventArgs() { player = this });

                Collected++;

                if (ItemPickUpEvent != null)
                    ItemPickUpEvent.Invoke(other.gameObject);
            }
        }
        #endregion
    }

}