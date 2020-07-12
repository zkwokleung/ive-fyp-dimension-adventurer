using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Enemies
{
    public class AttackManager : MonoBehaviour
    {
        public static AttackManager singleton;

        private List<AttackListener> listeners = new List<AttackListener>();

        private void Awake()
        {
            // Singleton
            if (singleton == null)
                singleton = this;
            else if (singleton != this)
            {
                Debug.Log($"{GetType().Name}: Instance already exists, destroying object . . . ");
                Destroy(gameObject);
            }
        }

        #region Public Methods
        public void RegisterListener(AttackListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(AttackListener listener)
        {
            listeners.Remove(listener);
        }

        public void RaiseAttack(WorldPosition position, float damage)
        {
            foreach (AttackListener al in listeners)
            {
                al.OnAttackRaise(position, damage);
            }
        }

        public void RaiseAttack(PlaneType plane, float damage)
        {
            for(int i = 0; i < 3; i++)
            {
                RaiseAttack(new WorldPosition(plane, (RunningTrack)i), damage);
            }
        }

        public void RaiseAttack(RunningTrack track, float damage)
        {
            for (int i = 0; i < 4; i++)
            {
                RaiseAttack(new WorldPosition((PlaneType)i, track), damage);
            }
        }
        #endregion
    }
}