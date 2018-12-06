using System.Collections.Generic;
using UnityEngine;

public class CObjectPool : MonoBehaviour
{
    public static CObjectPool m_sInstance;

    [Tooltip("Objects that will be add into the pool of GameObjects.")] [SerializeField] private List<GameObject> m_PooledPrefabs;
    [Tooltip("Initial amount of objects in the pool.")] [SerializeField] private int m_iPooledAmount = 5;
    [Tooltip("Boolean to decide if the pool will grow.")] [SerializeField] private bool m_bWillGrow = true;

    private List<GameObject> m_PooledObjects = null;

    private void Awake()
    {
        // Set m_sInstance for other Scripts to access
        m_sInstance = this;
        
        // Initiate List
        m_PooledObjects = new List<GameObject>();

        for (int i = 0; i < m_PooledPrefabs.Count; i++)
        {
            // Create the amount of the GameObjects needed to be add into pool
            for (int j = 0; j < m_iPooledAmount; j++)
            {
                GameObject obj = Instantiate(m_PooledPrefabs[i]);
                obj.SetActive(false);
                obj.transform.parent = transform;
                m_PooledObjects.Add(obj);
            }
        }
    }
    
    public GameObject GetPooledObject(GameObject Prefab)
    {
        // Check through the pool of objects for matching Tag and inactive GameObject
        for(int i = 0; i < m_PooledObjects.Count; i++)
        {
            if(!m_PooledObjects[i].activeSelf && m_PooledObjects[i].tag.Equals(Prefab.tag))
            {
                m_PooledObjects[i].SetActive(true);

                return m_PooledObjects[i];
            }
        }

        // When not found in the pool of objects and bWillGrow is true
        // Create the passed in Gameobject and Add into the pool
        if(m_bWillGrow)
        {
            GameObject obj = Instantiate(Prefab);
            m_PooledObjects.Add(obj);
            obj.SetActive(true);
            obj.transform.parent = transform;

            return obj;
        }

        return null;
    }
}