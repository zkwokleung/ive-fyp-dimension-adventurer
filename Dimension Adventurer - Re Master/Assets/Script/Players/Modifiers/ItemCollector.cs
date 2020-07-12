using UnityEngine;
using UnityEngine.Events;
using DimensionAdventurer.Items;
using System;
using DimensionAdventurer.Items.Effects;

namespace DimensionAdventurer.Players.Modifiers
{
    public class ItemCollector : MonoBehaviour
    {
        public EnvironmentData EnvironmentData;
        public Vector3 OffsetToPlayer;

        private Player player;
        private Vector3 _maxSize;

        private void Awake()
        {
            //Set to match to scale of the scene
            _maxSize = new Vector3(EnvironmentData.TileWidth, EnvironmentData.TileWidth, transform.localScale.z) * 2;

            transform.localScale = _maxSize;
        }

        void FixedUpdate()
        {
            transform.localPosition = player.transform.localPosition + OffsetToPlayer;
        }

        private void OnTriggerEnter(Collider other)
        {
            //When the gameobject collides with other objects
            if (other.CompareTag("Item"))
            {
                //Check if it is a collectable 
                ICollectable ic = other.GetComponent<ICollectable>();
                if (ic != null)
                    ic.OnCollected(gameObject, new CollectEventArgs() { player = player });
            }
        }

        public void OnCreated(Player player)
        {
            this.player = player;
        }
    }
}