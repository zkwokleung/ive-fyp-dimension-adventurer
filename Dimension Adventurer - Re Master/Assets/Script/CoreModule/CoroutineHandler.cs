using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer
{
    /// <summary>
    /// A singleton class that handles coroutine as a Key-Value Pair.
    /// Avoid running the coroutine more than once at the same time.
    /// </summary>
    public class CoroutineHandler : MonoBehaviour
    {
        private static CoroutineHandler singleton;
        /// <summary>
        /// A dictionary that contains references to enumerators, referencing the enumerator with a string tag
        /// </summary>
        private Dictionary<string, Coroutine> coroutines;

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

            coroutines = new Dictionary<string, Coroutine>();
        }

        void Update()
        {
            foreach (KeyValuePair<string, Coroutine> pair in coroutines)
            {
                if (pair.Value == null)
                    coroutines.Remove(pair.Key);
            }
        }

        #region Private Methods
        private void startCoroutine(string key, IEnumerator enumerator)
        {
            // If coroutine already exist, stop and remove it first.
            if (coroutines.ContainsKey(key))
            {
                StopCoroutine(key);
                coroutines.Remove(key);
            }

            // Start new coroutine
            coroutines.Add(key, StartCoroutine(enumerator));
        }

        private void stopCoroutine(string key)
        {
            if(!coroutines.ContainsKey(key))
            {
                Debug.Log($"CoroutineHandler: No coroutine with key {key} found.");
                return;
            }

            //Stops the coroutine
            base.StopCoroutine(coroutines[key]);

            //Remove it from the dictionary
            coroutines.Remove(key);
        }
        #endregion

        #region Public Static Methods
        public static void StartCoroutine(string key, IEnumerator enumerator)
        {
            if(singleton == null)
                return;

            singleton.startCoroutine(key, enumerator);
        }

        public static new void StopCoroutine(string key)
        {
            if (singleton == null)
                return;

            singleton.stopCoroutine(key);
        }
        #endregion
    }
}