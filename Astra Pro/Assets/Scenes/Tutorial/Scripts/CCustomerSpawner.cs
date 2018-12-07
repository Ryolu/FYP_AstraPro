using UnityEngine;
using System.Collections.Generic;

public class CCustomerSpawner : MonoBehaviour
{
    [Tooltip("Queue Point of Customer")] public Transform m_QueuePoint;

    [Tooltip("Spawn Point of Customer")] [SerializeField] private Transform m_SpawnPoint;
    [Tooltip("Prefab of Customer")] [SerializeField] private GameObject m_CustomerPrefab;
    [Tooltip("Time to wait before customer ordering food.")] [SerializeField] private float m_fOrderTiming = 3f;

    private Queue<CCustomer> m_CustomerQ;
    private float m_fWaitingTime = 0f;
    private int m_iCustomerCount = 0;

    private void Awake ()
    {
        // Initialise Queue
        m_CustomerQ = new Queue<CCustomer>();

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

        // Customer Queue Up
        m_CustomerQ.Enqueue(obj.GetComponent<CCustomer>());
    }

    private void Update()
    {
        // Customers order food 1 by 1 as they reached counter
        foreach (CCustomer customer in m_CustomerQ)
        {
            if (customer.m_bReachedTarget)
            {
                if (!customer.m_bOrderedFood)
                {
                    m_fWaitingTime += Time.deltaTime;
                    
                    // Wait for a few seconds(Let Customer "Think" before Ordering food)
                    if (m_fWaitingTime > m_fOrderTiming)
                    {
                        customer.OrderFood();
                        m_fWaitingTime = 0f;
                        m_CustomerQ.Dequeue();
                    }
                    else
                        break;
                }
            }
        }

    }
}
