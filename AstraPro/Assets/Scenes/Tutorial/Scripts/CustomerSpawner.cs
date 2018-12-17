using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance;

    [Tooltip("Queue Point of Customer")] [SerializeField] private Transform queuePoint;
    [Tooltip("Spawn Point of Customer")] [SerializeField] private Transform spawnPoint;
    [Tooltip("Prefab of Customer")] [SerializeField] private GameObject customerPrefab;
    [Tooltip("Time to wait before customer ordering food.")] [SerializeField] private float orderTiming = 3f;

    // For Ordering food 1 by 1
    private Queue<Customer> customerQueue;

    // Keep Track of current customers in queue(Not the data container, "Queue". But the queue in front of counter)
    public Dictionary<int, Customer> customerDic;

    private float waitingTime = 0f;
    public int customerCount = 0;

    private void Awake ()
    {
        // Set instance for other Scripts to access
        Instance = this;

        // Initialise Queue
        customerQueue = new Queue<Customer>();
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
        customerQueue.Enqueue(obj.GetComponent<Customer>());
        customerDic.Add(customerCount, obj.GetComponent<Customer>());
    }

    public void Leave(int customerId)
    {
        // Remove Customer with stated customerId
        customerCount -= 1;
        customerDic[customerId].Destroy();
        customerDic.Remove(customerId);

        // Only change customerId when Serving customer 1 or 2
        if (customerId == 1 || customerId == 2)
        {
            var newDic = new Dictionary<int, Customer>();

            switch (customerId)
            {
                case 1:
                    {
                        int newKey;
                        foreach (var item in customerDic)
                        {
                            // Change Key(customerId)
                            newKey = item.Key - 1;
                            item.Value.customerId = newKey;

                            // Recalculate the direction and target for customer to move towards
                            item.Value.CalculateDir();

                            // Copy Customer over to newDic
                            newDic.Add(newKey, item.Value);
                        }
                    }
                    break;
                case 2:
                    {
                        int newKey;
                        foreach (var item in customerDic)
                        {
                            if (item.Key == 1)
                            {
                                // Copy Customer over to newDic
                                newDic.Add(item.Key, item.Value);
                            }
                            else if (item.Key == 3)
                            {
                                // Change Key(customerId)
                                newKey = item.Key - 1;
                                item.Value.customerId = newKey;

                                // Recalculate the direction and target for customer to move towards
                                item.Value.CalculateDir();

                                // Copy Customer over to newDic
                                newDic.Add(newKey, item.Value);
                            }
                        }
                    }
                    break;
            }
            
            // Copy newDic into our customerDic
            customerDic = newDic;
        }

    }

    private void Update()
    {
        // Customers order food 1 by 1 as they reached counter
        for(int i = 0; i < customerQueue.Count; i++)
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

        // Debug KeyPress for serve customer
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            // serve id 1
            Leave(1);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            // serve id 2
            Leave(2);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            // serve id 3
            Leave(3);
        }

    }
}
