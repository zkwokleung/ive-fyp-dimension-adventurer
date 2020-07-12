using UnityEngine;
using System.Collections;
using DimensionAdventurer.Items.Effects;

namespace DimensionAdventurer.Items
{
    [RequireComponent(typeof(Collider))]
    public class Item : MonoBehaviour, IPooledObject, ICollectable
    {
        public ItemData data;
        public bool Collected { get; protected set; }
        [HideInInspector] public WorldObjectManager game;

        #region MonoBehaviour
        private void Update()
        {
            transform.Rotate(new Vector3(Random.Range(0, 5f), Random.Range(0f, 5f), Random.Range(0f, 5f)));
        }

        private void OnEnable()
        {
            //Replaced by OnObjectSpawn
        }


        private void OnDisable()
        {
            if (game == null)
                return;

            game.Items.Remove(gameObject);
            game = null;
        }

        #endregion

        #region Item
        private IEnumerator Collect(GameObject source)
        {
            Vector3 target = source.gameObject.transform.position + (source.gameObject.transform.up * -1);
            while (Vector3.Distance(transform.localPosition, target) > 1f)
            {
                transform.localPosition= Vector3.Lerp(transform.localPosition, target, 10 * Time.deltaTime);
                yield return null;
            }

            gameObject.SetActive(false);
        }

        #endregion

        #region Interface
        public void OnObjectSpawn()
        {
            //A random rotation when the item is spawned
            transform.localEulerAngles = new Vector3(Random.Range(-360f, 360f), Random.Range(-360f, 360f), Random.Range(-360f, 360f));

            //Reset the collected state
            Collected = false;
        }

        public void OnCollected(GameObject source, CollectEventArgs e)
        {
            if (Collected)
                return;

            Collected = true;
            StartCoroutine(Collect(source));
            data.Use(source, new ItemEffectEventArgs()
            {
                player = e.player,
                Producer = this
            });
        }
        #endregion
    }
}