using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [Tooltip("Objects that will be add into the pool of GameObjects.")] [SerializeField] private List<GameObject> pooledPrefabs;
    [Tooltip("Initial amount of objects in the pool.")] [SerializeField] private int pooledAmount = 3;
    [Tooltip("Boolean to decide if the pool will grow.")] [SerializeField] private bool willGrow = true;

    private List<GameObject> pooledObjects = null;

    private void Awake()
    {
        // Set instance for other Scripts to access
        Instance = this;
        
        // Initiate List
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < pooledPrefabs.Count; i++)
        {
            // Create the amount of the GameObjects needed to be add into pool
            for (int j = 0; j < pooledAmount; j++)
            {
                var obj = Instantiate(pooledPrefabs[i]);
                obj.SetActive(false);
                obj.transform.parent = transform;
                pooledObjects.Add(obj);
            }
        }
    }
    
    public GameObject GetPooledObject(GameObject prefab)
    {
        // Check through the pool of objects for matching Tag and inactive GameObject
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if(!pooledObjects[i].activeSelf && pooledObjects[i].tag.Equals(prefab.tag))
            {
                pooledObjects[i].SetActive(true);

                return pooledObjects[i];
            }
        }

        // When not found in the pool of objects and willGrow is true
        // Create the passed in Gameobject and Add into the pool
        if(willGrow)
        {
            var obj = Instantiate(prefab);
            pooledObjects.Add(obj);
            obj.SetActive(true);
            obj.transform.parent = transform;

            return obj;
        }

        return null;
    }
}