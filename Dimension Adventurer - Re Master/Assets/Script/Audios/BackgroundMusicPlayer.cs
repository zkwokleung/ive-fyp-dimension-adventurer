using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Audios
{
    public class BackgroundMusicPlayer : MonoBehaviour
    {
        public static BackgroundMusicPlayer singleton { get; private set; }

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

            DontDestroyOnLoad(gameObject);
        }
    }
}