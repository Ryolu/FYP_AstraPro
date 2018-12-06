using UnityEngine;

public class CCustomerSpawner : MonoBehaviour
{
    //public static CCustomerSpawner m_sInstance;

    [Tooltip("Queue Point of Customer")] public Transform m_QueuePoint;

    [Tooltip("Spawn Point of Customer")] [SerializeField] private Transform m_SpawnPoint;
    [Tooltip("Prefab of Customer")] [SerializeField] private GameObject m_CustomerPrefab;

    private int m_iCustomerCount = 0;

    //private void Awake()
    //{
    //    // Set m_sInstance for other Scripts to access
    //    m_sInstance = this;
    //}

    private void Start ()
    {
        // Spawn 3 customers at start of game
        for (int i = 0; i < 3; i++)
        {
            NewCustomer();
        }
	}
	
    // Create/Pull a new Customer
    private void NewCustomer()
    {
        GameObject obj = CObjectPool.m_sInstance.GetPooledObject(m_CustomerPrefab);

        if (!obj) return;
        
        obj.transform.position = m_SpawnPoint.position;
        obj.transform.rotation = Quaternion.identity;

        m_iCustomerCount++;

        // Set variables for each new Customer
        obj.GetComponent<CCustomer>().m_iCustomerCount = m_iCustomerCount;
        obj.GetComponent<CCustomer>().m_QueuePosition = m_QueuePoint.position;
    }

}
