using UnityEngine;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance;

    [Tooltip("Queue Point of Customer")] [SerializeField] private Transform queuePoint;
    [Tooltip("Spawn Point of Customer")] [SerializeField] private Transform spawnPoint;
    [Tooltip("Prefab of Customer")] [SerializeField] private GameObject customerPrefab;
    //[Tooltip("Time to wait before customer ordering food.")] [SerializeField] private float orderTiming = 0f;

    // For Ordering food 1 by 1
    //private Queue<Customer> customerQueue;
    //private float waitingTime = 0f;

    // Keep Track of current customers in queue(Not the data container, "Queue". But the queue in front of counter)
    [HideInInspector] public Dictionary<int, Customer> customerDic;
    [HideInInspector] public int customerCount = 0;

    private void Awake ()
    {
        // Set instance for other Scripts to access
        Instance = this;

        // Initialise Queue
        //customerQueue = new Queue<Customer>();
        customerDic = new Dictionary<int, Customer>();

        // Spawn 3 customers at start of game
        for (int i = 0; i < 3; i++)
        {
            NewCustomer();
        }
	}
	
    // Create/Pull a new Customer
    private void NewCustomer()
    {
        var obj = ObjectPool.Instance.GetPooledObject(customerPrefab);

        if (!obj) return;
        
        obj.transform.position = spawnPoint.position;
        obj.transform.rotation = Quaternion.Euler(0, 180, 0);

        customerCount += 1;

        // Set variables for each new Customer
        obj.GetComponent<Customer>().customerId = customerCount;
        obj.GetComponent<Customer>().queuePosition = queuePoint.position;

        // Customer Queue Up
        //customerQueue.Enqueue(obj.GetComponent<Customer>());
        customerDic.Add(customerCount, obj.GetComponent<Customer>());
    }

    //private void Update()
    //{
    //    //// Customers order food 1 by 1 as they reached counter
    //    //for (int i = 0; i < customerQueue.Count; i++)
    //    //{
    //    //    if (customerQueue.ElementAt(i).reachedTarget)
    //    //    {
    //    //        if (!customerQueue.ElementAt(i).orderedFood)
    //    //        {
    //    //            customerQueue.ElementAt(i).OrderFood((Customer.FoodOrder)Random.Range(1, 5));
    //    //            customerQueue.Dequeue();

    //    //            //waitingTime += Time.deltaTime;

    //    //            //// Wait for a few seconds(Let Customer "Think" before Ordering food)
    //    //            //if (waitingTime > orderTiming)
    //    //            //{
    //    //            //    customerQueue.ElementAt(i).OrderFood();
    //    //            //    waitingTime = 0f;
    //    //            //    customerQueue.Dequeue();
    //    //            //}
    //    //            //else
    //    //            //{
    //    //            //    break;
    //    //            //}
    //    //        }
    //    //    }
    //    //}
    //}
}
