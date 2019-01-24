using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance;

    [Tooltip("Spawn Point of Customer")] public Transform spawnPoint;

    [Tooltip("Queue Point of Customer")] [SerializeField] private Transform queuePoint;
    [Tooltip("Prefabs of Customers")] [SerializeField] private List<GameObject> customerPrefabs;
    //[Tooltip("Time to wait before customer ordering food.")] [SerializeField] private float orderTiming = 0f;

    // For Ordering food 1 by 1
    //private Queue<Customer> customerQueue;
    //private float waitingTime = 0f;

    // Keep Track of current customers in queue(Not the data container, "Queue". But the queue in front of counter)
    [HideInInspector] public Dictionary<int, Customer> customerDic;
    [HideInInspector] public int customerCount = 0;

    private float elapsedTime;
    private float endTime = 3f;

    private void Awake ()
    {
        // Set instance for other Scripts to access
        Instance = this;

        // Initialise Queue
        //customerQueue = new Queue<Customer>();
        customerDic = new Dictionary<int, Customer>();

        // Spawn 1 customer at start of game
        NewCustomer(1);
	}
	
    // Randomly choose a customer prefab for NewCustomer(int count) to spawn
    private GameObject RandomCustomer()
    {
        var rand = Random.Range(0, 4);

        return customerPrefabs[rand];
    }

    // Create/Pull a new Customer
    private void NewCustomer(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var obj = ObjectPool.Instance.GetPooledObject(RandomCustomer());

            if (!obj) return;

            obj.transform.position = spawnPoint.position;
            obj.transform.rotation = Quaternion.Euler(0, 180, 0);
            //obj.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, -1), Vector3.up);

            customerCount += 1;

            // Set variables for each new Customer
            obj.GetComponent<Customer>().customerId = customerCount;
            obj.GetComponent<Customer>().queuePosition = queuePoint.position;
            obj.GetComponent<Customer>().InitiateData();

            // Customer Queue Up
            //customerQueue.Enqueue(obj.GetComponent<Customer>());
            customerDic.Add(customerCount, obj.GetComponent<Customer>());
        }
    }

    private void Update()
    {
        if(!Menu_Manager.Instance.Tutorial_Mode)
        {
            if (PauseManager.Instance != null && PauseManager.Instance.isPaused) return;

            if (customerDic.Any(x => x.Value.fighting == true)) return;

            elapsedTime += Time.deltaTime;
            if (elapsedTime >= endTime)
            {
                elapsedTime = 0f;

                // Spawn Customer
                if (customerCount < 3)
                {
                    NewCustomer(1);
                }
            }            
        }
        #region Order Food 1 by 1 (Comment-ed)
        //// Customers order food 1 by 1 as they reached counter
        //for (int i = 0; i < customerQueue.Count; i++)
        //{
        //    if (customerQueue.ElementAt(i).reachedTarget)
        //    {
        //        if (!customerQueue.ElementAt(i).orderedFood)
        //        {
        //            customerQueue.ElementAt(i).OrderFood((Customer.FoodOrder)Random.Range(1, 5));
        //            customerQueue.Dequeue();

        //            //waitingTime += Time.deltaTime;

        //            //// Wait for a few seconds(Let Customer "Think" before Ordering food)
        //            //if (waitingTime > orderTiming)
        //            //{
        //            //    customerQueue.ElementAt(i).OrderFood();
        //            //    waitingTime = 0f;
        //            //    customerQueue.Dequeue();
        //            //}
        //            //else
        //            //{
        //            //    break;
        //            //}
        //        }
        //    }
        //}
        #endregion Order Food 1 by 1 (Comment-ed)
    }
}
