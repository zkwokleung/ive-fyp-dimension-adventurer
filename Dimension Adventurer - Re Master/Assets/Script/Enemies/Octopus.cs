using DimensionAdventurer.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Enemies
{
    public class Octopus : Enemy
    {
        #region Enemy
        public override void OnPlayerHitted(Player player)
        {
            Debug.Log("Attack Success");
            //Check what the last action is, decrease anger accordingly.
            switch (LastAction)
            {
                case EnemyAction.Howl:
                    DecreaseAnger(6);
                    break;

                case EnemyAction.Roar:
                    DecreaseAnger(9);
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region IEnumerator
        protected override IEnumerator IEAttack()
        {
            yield return IEAttackRandomPosition(atkDelay);
            currentCoroutine = null;
        }

        protected override IEnumerator IEHowl()
        {
            Player[] players = GameManager.GetAllPlayer();
            Player target;
            if (players.Length > 1)
                target = players[Random.Range(0, players.Length)];
            else
                target = players[0];

            yield return IEAttackTargetPlane(target.WorldPosition.Plane, atkDelay);
            currentCoroutine = null;
        }

        protected override IEnumerator IERoar()
        {
            for(int i = 0; i < 4; i++)
            {
                StartCoroutine(IEAttackTargetPlane((PlaneType)i, atkDelay));
                yield return new WaitForSeconds(0.5f);
            }
            currentCoroutine = null;
        }
        #endregion
    }
}