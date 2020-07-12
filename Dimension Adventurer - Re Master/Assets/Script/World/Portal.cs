using System;
using UnityEngine;

namespace DimensionAdventurer.World
{
    public class Portal : MonoBehaviour
    {
        public static event Action PortalEnteredEvent;
        [SerializeField] protected EnvironmentData EnvironmentData;


        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (PortalEnteredEvent != null)
                PortalEnteredEvent.Invoke();
        }
    }
}