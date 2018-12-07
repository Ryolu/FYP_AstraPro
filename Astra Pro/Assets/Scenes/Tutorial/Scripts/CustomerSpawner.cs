using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CustomerSpawner : MonoBehaviour
{
    [Tooltip("Queue Point of Customer")] public Transform queuePoint;

    [Tooltip("Spawn Point of Customer")] [SerializeField] private Transform spawnPoint;
    [Tooltip("Prefab of Customer")] [SerializeField] private GameObject customerPrefab;
    [Tooltip("Time to wait before customer ordering food.")] [SerializeField] private float orderTiming = 3f;

    private Queue<Customer> customerQueue;
    private float waitingTime = 0f;
    private int customerCount = 0;

    private void Awake ()
    {
        // Initialise Queue
        customerQueue = new Queue<Customer>();

        // Spawn 3 customers at start of game
        for (int i = 0; i < 3; i++)
        {
            NewCustomer();
        }
	}
	
    // Create/Pull a new Customer
    private void NewCustomer()
    {
        var obj = ObjectPool.instance.GetPooledObject(customerPrefab);

        if (!obj) return;
        
        obj.transform.position = spawnPoint.position;
        obj.transform.rotation = Quaternion.identity;

        customerCount++;

        // Set variables for each new Customer
        obj.GetComponent<Customer>().customerCount = customerCount;
        obj.GetComponent<Customer>().queuePosition = queuePoint.position;

        // Customer Queue Up
        customerQueue.Enqueue(obj.GetComponent<Customer>());
    }

    private void Update()
    {
        // Customers order food 1 by 1 as they reached counter
        for(int i = 0; i < customerQueue.Count; i++) // (CCustomer customer in m_CustomerQ)
        {
            if ( customerQueue.ElementAt(i).reachedTarget)
            {
                if (!customerQueue.ElementAt(i).orderedFood)
                {
                    waitingTime += Time.deltaTime;
                    
                    // Wait for a few seconds(Let Customer "Think" before Ordering food)
                    if (waitingTime > orderTiming)
                    {
                        customerQueue.ElementAt(i).OrderFood();
                        waitingTime = 0f;
                        customerQueue.Dequeue();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

    }
}
