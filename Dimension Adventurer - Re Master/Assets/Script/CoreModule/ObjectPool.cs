using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer
{
    public class ObjectPool : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        #region Static
        public static ObjectPool singleton;
        #endregion

        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;
        public Transform defaultParent;

        #region MonoBehaviour
        private void Awake()
        {
            singleton = this;
        }

        void Start()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject go = Instantiate(pool.prefab);
                    go.transform.SetParent(defaultParent);
                    go.SetActive(false);
                    objectPool.Enqueue(go);
                }

                poolDictionary.Add(pool.tag, objectPool);
            }
        }

        #endregion

        public GameObject Spawn(string tag, Vector3 position)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogError("No object named " + tag + " was found.");
                return null;
            }

            GameObject obj = poolDictionary[tag].Dequeue();

            obj.SetActive(true);
            obj.transform.position = position;

            //OnObjectSpawn
            IPooledObject ipo = obj.GetComponent<IPooledObject>();

            if (ipo != null)
                ipo.OnObjectSpawn();

            poolDictionary[tag].Enqueue(obj);

            return obj;
        }

        public void Despawn(GameObject gameObject)
        {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(defaultParent);
        }
    }
}