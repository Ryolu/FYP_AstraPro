using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// This class manages Spawning algorithm of Customer and holds Dictionary of Customers.
/// 
/// Can be found attached in CustomerSpawner.
/// </summary>
public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance;
   
    /// <summary>
    /// A list of GameObjects(Prefabs of Customers).
    /// </summary>
    [Tooltip("Prefabs of Customers.\n" +
        "\n" +
        "Can be found in Assets -> Prefabs -> Customer")]
    [SerializeField] private List<GameObject> customerPrefabs;

    /// <summary>
    /// A float that determines the Delay time of spawning customer.
    /// 
    /// Default: 10
    /// </summary>
    [Tooltip("A float that determines the Delay time of spawning customer.\n" +
        "Default: 10")]
    [SerializeField] private float spawnDelay = 10f;
    
    /// <summary>
    /// A Dictionary of Customers with CustomerID as the Key.
    /// </summary>
    [HideInInspector] public Dictionary<int, Customer> customerDic;

    /// <summary>
    /// Total count of Customer active in the game.
    /// 
    /// Default: 0
    /// </summary>
    [HideInInspector] public int customerCount = 0;

    /// <summary>
    /// Spawn Point of Customer.
    /// </summary>
    [HideInInspector] public Transform spawnPoint;

    /// <summary>
    /// Queue Point of Customer.
    /// </summary>
    private Transform queuePoint;

    /// <summary>
    /// A float timer used to spawn customer by spawnDelay above
    /// </summary>
    private float elapsedTime;

    /// <summary>
    /// A boolean that determines to spawn Customer in Tutorial Scene or not.
    /// 
    /// Default: False
    /// </summary>
    private bool spawnedGuideCustomer = false;

    private void Awake ()
    {
        // Set instance for other Scripts to access
        Instance = this;

        // Initialise Queue
        customerDic = new Dictionary<int, Customer>();

        // Initialise Transform points
        spawnPoint = transform.GetChild(0);
        queuePoint = transform.GetChild(1);
	}

    /// <summary>
    /// Randomly choose a customer prefab for NewCustomer(int count) to spawn
    /// </summary>
    /// <returns> A Customer GameObject </returns>
    private GameObject RandomCustomer()
    {
        var rand = Random.Range(0, 4);

        return customerPrefabs[rand];
    }

    /// <summary>
    /// Creates or Pulls a new Customer from The Object Pool.
    /// Reset its Position and Rotation.
    /// Updates Customer Count and Customer Dictionary.
    /// </summary>
    private void NewCustomer()
    {
        var obj = ObjectPool.Instance.GetPooledObject(RandomCustomer());

        // If obj is Null, do not do anything
        if (!obj)
        {
            return;
        }

        // Resets Position and Rotation of Customer
        obj.transform.position = spawnPoint.position;
        obj.transform.rotation = Quaternion.Euler(0, 180, 0);

        // Increase Customer Count
        customerCount += 1;

        // Set variables for each new Customer
        obj.GetComponent<Customer>().customerId = customerCount;
        obj.GetComponent<Customer>().queuePosition = queuePoint.position;
        obj.GetComponent<Customer>().InitiateData();

        // Add Customer into Dictionary
        customerDic.Add(obj.GetComponent<Customer>().customerId, obj.GetComponent<Customer>());
    }

    private void Update()
    {
        // If the game is not in Tutorial Scene
        if(!Menu_Manager.Instance.Tutorial_Mode)
        {
            // If the game is in Pause Status, do not do anything
            if (PauseManager.Instance != null && PauseManager.Instance.isPaused)
            {
                return;
            }

            // If any of the Active Customers are fighting with Player, do not do anything
            if (customerDic.Any(x => x.Value.fighting == true))
            {
                return;
            }

            // Runs the Spawning Timer
            elapsedTime += Time.deltaTime;

            // When Timer Reach limit
            if (elapsedTime >= spawnDelay)
            {
                // Reset Timer
                elapsedTime = 0f;

                // Spawn Customer if total count is less than 3
                if (customerCount < 3)
                {
                    NewCustomer();
                }
            }            
        }
        else
        {
            // If already spawned a Customer for Tutorial, do not do anything anymore
            if(spawnedGuideCustomer)
            {
                return;
            }

            // Runs the Spawning Timer
            elapsedTime += Time.deltaTime;

            // When Timer Reach limit
            if (elapsedTime >= spawnDelay)
            {
                // Reset Timer
                elapsedTime = 0f;

                // Spawn Customer if total count is less than 3
                if (customerCount < 3)
                {
                    NewCustomer();
                    spawnedGuideCustomer = true;
                }
            }
        }
    }
}