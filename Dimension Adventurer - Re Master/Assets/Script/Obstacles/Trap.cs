using DimensionAdventurer.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Obstacles
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] private float damage = 1f;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            other.GetComponent<Player>().Damage(damage);
        }
    }
}