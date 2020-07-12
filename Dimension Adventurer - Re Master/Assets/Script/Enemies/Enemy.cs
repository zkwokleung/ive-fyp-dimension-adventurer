using DimensionAdventurer.Players;
using DimensionAdventurer.World;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DimensionAdventurer.Enemies
{
    public enum EnemyAction
    {
        Attack,
        Howl,
        Roar,
        NoAction
    }

    /// <summary>
    /// When creating a new enemy, make a class which inherit this class.
    /// </summary>
    [Serializable]
    public class Enemy : MonoBehaviour, IAbility
    {
        #region Static
        public static int ANGER_CHECK_EVERY_ATTACK = 3;
        /// <summary>
        /// A dictionary which stores all the enemy created.
        /// </summary>
        public static Dictionary<string, Enemy> EnemiesDictonary;
        #endregion

        [SerializeField] protected GameObject Model;

        [Header("Attributes")]
        public float Damage;
        public float MinAtkCooldown;
        public float MaxAtkCooldown;

        /// <summary>
        /// The time waits before attacking the player.
        /// </summary>
        [SerializeField] protected float atkDelay = 1.5f;

        /// <summary>
        /// The current cooldown for attacking.
        /// </summary>
        [SerializeField] protected float Cooldown;

        /// <summary>
        /// How many times did the Enemy attacked.
        /// </summary>
       protected int AtkCount;

        /// <summary>
        /// Is the enemy attacking.
        /// </summary>
        protected bool IsAttacking =  false;

        /// <summary>
        /// The last action of the enemy.
        /// </summary>
        public EnemyAction LastAction { get; protected set; } = EnemyAction.NoAction;

        /// <summary>
        /// The anger level of the enemy. Range between 0(inclusive) to 10(inclusive)
        /// </summary>
        public int Anger
        {
            get => _anger;
            protected set
            {
                //Anger must be range 0 to 10
                if (value <= 0)
                    _anger = 0;
                else if (value >= 10)
                    _anger = 10;
                else
                    _anger = value;
            }
        }
        private int _anger = 0;

        protected IEnumerator currentCoroutine;

        #region MonoBehaviour
        protected virtual void Awake()
        {
            //Set up static dictionary and player reference
            if(EnemiesDictonary == null)
                EnemiesDictonary = new Dictionary<string, Enemy>();
        }

        protected virtual void OnEnable()
        {
            GameManager.GameOverEvent += OnGameOver;
            Portal.PortalEnteredEvent += OnPlayerEnterPortal;
            Player.PlayerDamagedEvent += OnPlayerHitted;
        }

        private void OnDisable()
        {
            GameManager.GameOverEvent -= OnGameOver;
            Portal.PortalEnteredEvent -= OnPlayerEnterPortal;
            Player.PlayerDamagedEvent -= OnPlayerHitted;
        }

        protected virtual void Start()
        {
            Cooldown = RandomCooldown();
        }

        protected virtual void Update()
        {
            IsAttacking = (currentCoroutine != null);

            // A simple FNS Machine
            if (!IsAttacking)
            {
                // Check cooldown
                if (Cooldown <= 0)
                {
                    string atkMsg = "";
                    // Check attack count, if the enemy should use ability
                    if (AtkCount % ANGER_CHECK_EVERY_ATTACK == 0 && AtkCount != 0)
                    {
                        switch (EvaluateAnger())
                        {
                            case EnemyAction.Attack:
                                atkMsg = "Anger Attack";
                                Attack();
                                break;

                            case EnemyAction.Howl:
                                atkMsg = ("Howl");
                                Howl();
                                break;

                            case EnemyAction.Roar:
                                atkMsg =  ("Roar");
                                Roar();
                                break;
                        }
                        // Reset Attack count
                        AtkCount = 0;
                    }
                    else
                    {
                        //Normal Attack
                        atkMsg = "Basic attack";
                        Attack();
                        AtkCount++;
                    }
                    // Log the attack
                    Debug.Log($"{this.name}: {atkMsg}");

                    //Reset the cooldown of attack
                    ResetCooldown();
                }
                else
                {
                    //Cooldown
                    Cooldown -= Time.deltaTime;
                    //Debug.Log(Cooldown);
                }
            }
        }

        #endregion

        #region Protected Method
        protected float RandomCooldown()
        {
            return Random.Range(MinAtkCooldown, MaxAtkCooldown);
        }

        protected virtual void ResetCooldown()
        {
            Cooldown = Random.Range(MinAtkCooldown, MaxAtkCooldown);
        }

        /// <summary>
        /// Check anger and return the action of what the enemy should do
        /// </summary>
        protected virtual EnemyAction EvaluateAnger()
        {
            if (Anger < 5)
            {
                //Attack
                return EnemyAction.Attack;
            }
            else if (Anger < 8)
            {
                //Howl
                return EnemyAction.Howl;
            }
            else
            {
                //Roar
                return EnemyAction.Roar;
            }
        }

        /// <summary>
        /// Increase the anger of the enemy
        /// </summary>
        /// <param name="amont"></param>
        protected virtual void IncreaseAnger(int amont)
        {
            Anger += amont;
        }

        /// <summary>
        /// Decrease the anger of the enemy
        /// </summary>
        /// <param name="amont"></param>
        protected virtual void DecreaseAnger(int amont)
        {
            Anger -= amont;
        }
        #endregion

        #region IAbility
        public virtual void Attack()
        {
            if (currentCoroutine != null)
            {
                Debug.Log("The enemy is busy but still trying to attack.");
                return;
            }

            currentCoroutine = IEAttack();
            StartCoroutine(currentCoroutine);
            IncreaseAnger(1);
            LastAction = EnemyAction.Attack;
        }

        public virtual void Howl()
        {
            if (currentCoroutine != null)
            {
                Debug.Log("The enemy is busy but still trying to attack.");
                return;
            }

            currentCoroutine = IEHowl();
            StartCoroutine(currentCoroutine);
            IncreaseAnger(1);

            LastAction = EnemyAction.Howl;
        }

        public virtual void Roar()
        {
            if (currentCoroutine != null)
            {
                Debug.Log("The enemy is busy but still trying to attack.");
                return;
            }

            currentCoroutine = IERoar();
            StartCoroutine(currentCoroutine);
            IncreaseAnger(1);

            LastAction = EnemyAction.Roar;
        }
        #endregion

        #region Enumerators
        protected virtual IEnumerator IEAttack()
        {
            yield return null;
        }

        protected virtual IEnumerator IEHowl()
        {
            yield return null;
        }

        protected virtual IEnumerator IERoar()
        {
            yield return null;
        }

        protected virtual IEnumerator IEAttackTargetPosition(WorldPosition position, float delay)
        {
            yield return AttackIndicatorManager.singleton.GetIndicator(position).IEDisplay(delay);

            AttackManager.singleton.RaiseAttack(position, Damage);
        }

        protected virtual IEnumerator IEAttackTargetPlane(PlaneType plane, float delay)
        {
            foreach (AttackIndicator ai in AttackIndicatorManager.singleton.GetIndicator(plane))
            {
                ai.Display(delay);
            }
            yield return new WaitForSeconds(delay);

            AttackManager.singleton.RaiseAttack(plane, Damage);
        }

        protected virtual IEnumerator IEAttackRandomPosition(float delay)
        {
            //Random a position to attack
            WorldPosition atkPos = WorldPosition.RandomPosition();
            //Debug.Log(atkPos.ToString());

            //Shows the indicator and wait for seconds before attacking
            yield return AttackIndicatorManager.singleton.GetIndicator(atkPos).IEDisplay(delay);

            AttackManager.singleton.RaiseAttack(atkPos, Damage);
        }

        protected virtual IEnumerator IEAttackRandomPlane(float delay)
        {
            PlaneType atkPlane = WorldPosition.RandomPlane();

            foreach (AttackIndicator ai in AttackIndicatorManager.singleton.GetIndicator(atkPlane))
            {
                ai.Display(delay);
            }
            yield return new WaitForSeconds(delay);

            AttackManager.singleton.RaiseAttack(atkPlane, Damage);
        }
        #endregion

        #region Event
        private void OnPlayerEnterPortal()
        {
            StopAllCoroutines();
            currentCoroutine = null;
        }

        public virtual void OnPlayerHitted(Player player)
        {

        }

        public virtual void OnGameOver(string reason)
        {
            this.enabled = false;
        }
        #endregion

        #region Static
        public static void ChangeEnemy(string name)
        {
            //Disable all enemy
            foreach(Enemy e in new List<Enemy>(EnemiesDictonary.Values))
            {
                e.gameObject.SetActive(false);
            }

            //Active selected enemy
            Enemy enemy;
            if (EnemiesDictonary.TryGetValue(name, out enemy))
            {
                enemy.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("No Enemy named " + name + "was found.");
            }
        }

        public static Enemy GetEnemy(string name)
        {
            Enemy e;
            if (EnemiesDictonary.TryGetValue(name, out e))
                return e;
            else
                return null;
        }
        #endregion
    }

}