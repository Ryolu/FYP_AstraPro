using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class manages The Object Pool and allow Spawning of GameObject.
/// 
/// Can be found attached in ObjectPool.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    /// <summary>
    /// GameObjects that needs to be prepared in the pool.
    /// </summary>
    [Tooltip("GameObjects that needs to be prepared in the pool.")]
    [SerializeField] private List<GameObject> pooledPrefabs;

    [Header("Pool Data")]
    [Space(5)]

    /// <summary>
    /// Initial amount of each object to prepare in the pool.
    /// 
    /// Default: 3
    /// </summary>
    [Tooltip("Initial amount of each object to prepare in the pool.\n" + "Default: 3")]
    [SerializeField] private int pooledAmount = 3;

    /// <summary>
    /// A boolean that determines if the pool will increase in size.
    /// 
    /// Default: True
    /// </summary>
    [Tooltip("A boolean that determines if the pool will increase in size.\n" + "Default: True")]
    [SerializeField] private bool willGrow = true;

    /// <summary>
    /// A list containing all the prepared GameObjects. a.k.a The Object Pool.
    /// 
    /// Default: Null
    /// </summary>
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
    
    /// <summary>
    /// Used in other Scripts when one needs to spawn a GameObject.
    /// </summary>
    /// <param name="prefab"> The GameObject that needs to be spawn. </param>
    /// <returns> The spawned GameObject. </returns>
    public GameObject GetPooledObject(GameObject prefab)
    {
        // Check through The Object Pool for matching Tag and inactive GameObject
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if(!pooledObjects[i].activeSelf && pooledObjects[i].tag.Equals(prefab.tag))
            {
                // Activates the found GameObject and return it
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