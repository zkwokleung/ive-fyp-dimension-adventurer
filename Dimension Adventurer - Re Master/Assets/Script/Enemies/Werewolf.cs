using DimensionAdventurer.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Enemies
{
    public class Werewolf : Enemy
    {
        public bool IsHowl = false;
        public float HowlAtkCD = 1f;
        public float HowlDuration = 6f;

        #region MonoBehaviour

        #endregion

        #region Enemy
        public override void OnPlayerHitted(Player player)
        {
            switch (LastAction)
            {
                case EnemyAction.Attack:
                    if (IsHowl)
                        DecreaseAnger(6);
                    break;

                case EnemyAction.Roar:
                    DecreaseAnger(9);
                    break;
            }
        }

        protected override void ResetCooldown()
        {
            //Addtional: when howl is active, cooldown reduce to 1s
            if (IsHowl)
                Cooldown = HowlAtkCD;
            else
                base.ResetCooldown();
        }
        #endregion

        #region IEnumerators
        protected override IEnumerator IEAttack()
        {
            yield return IEAttackRandomPosition(atkDelay);
            currentCoroutine = null;
        }

        protected override IEnumerator IEHowl()
        {
            StartCoroutine(IEActiveHowl(HowlDuration));
            //Return the time for playing animation. Currently no animations.
            yield return null;
            currentCoroutine = null;
        }

        protected override IEnumerator IERoar()
        {
            for (int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    StartCoroutine(IEAttackTargetPosition(new WorldPosition((PlaneType)i, (RunningTrack)j), atkDelay));
                    yield return new WaitForSeconds(0.5f);
                }
            }
            currentCoroutine = null;
        }

        private IEnumerator IEActiveHowl(float seconds)
        {
            IsHowl = true;
            yield return new WaitForSeconds(seconds);
            IsHowl = false;
        }
        #endregion
    }
}