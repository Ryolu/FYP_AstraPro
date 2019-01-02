using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Customer : MonoBehaviour
{
    [Tooltip("Movement Speed of Customer")] [SerializeField] private float movementSpeed = 2.5f;
    [Tooltip("Rotate Speed of Customer")] [SerializeField] private float rotateSpeed = 80f;
    [Tooltip("How long the Customer will wait for food")] [SerializeField] private float waitTiming = 30f;
    [Tooltip("Timer Filler Image")] [SerializeField] private Image timerImage;
    [Tooltip("Foods that customer can order")] [SerializeField] private FoodSO[] foodOrder;
    [Tooltip("Tableware Prefab to spawn when order food")] [SerializeField] private GameObject bowl;
    [Tooltip("Tableware Prefab to spawn when order food")] [SerializeField] private GameObject plate;

    [HideInInspector] public Vector3 queuePosition;
    [HideInInspector] public int customerId;
    [HideInInspector] public bool reachedTarget = false;
    [HideInInspector] public bool orderedFood = false;
    [HideInInspector] public FoodSO foodOrdered;

    private Vector3 dir;
    private Vector3 targetPosition;
    private float customerSizeX;
    private Gradient greenYellowGradient;
    private Gradient yellowRedGradient;

    private void Start()
    {
        customerSizeX = transform.lossyScale.x * 25f;
        waitTiming = Random.Range((waitTiming / 3), waitTiming);

        CalculateDir();
        InitiateColor();
    }

    // Initiate Gradients, which is used to change color based on fillAmount of timerImage
    private void InitiateColor()
    {
        greenYellowGradient = new Gradient();
        var ck1 = new GradientColorKey[2];
        ck1[0].color = Color.green;
        ck1[0].time = 1f;

        ck1[1].color = Color.yellow;
        ck1[1].time = 0.5f;

        var ak1 = new GradientAlphaKey[0];

        greenYellowGradient.SetKeys(ck1, ak1);

        yellowRedGradient = new Gradient();
        var ck2 = new GradientColorKey[2];
        ck2[0].color = Color.yellow;
        ck2[0].time = 0.5f;

        ck2[1].color = Color.red;
        ck2[1].time = 0f;

        var ak2 = new GradientAlphaKey[0];

        yellowRedGradient.SetKeys(ck2, ak2);
    }

    private void SetTableWareData()
    {
        var spawnPoint = transform.GetChild(2);
        var obj = transform.GetChild(3);
        obj.transform.localPosition = spawnPoint.localPosition;
        obj.transform.eulerAngles = spawnPoint.eulerAngles;
        obj.transform.localScale = spawnPoint.localScale;
    }

    // Calculate Direction for customer to move and Record down the Target position based on number of customer
    public void CalculateDir()
    {
        // Force to false, to allow movement towards target
        if(reachedTarget && movementSpeed < 2.5f)
        {
            reachedTarget = false;
            movementSpeed = 2.5f;
        }

        if(customerId == 1)
        {
            targetPosition = queuePosition;
            dir = (queuePosition - transform.position).normalized;
        }
        else
        {
            // Later customer walk slower
            movementSpeed = movementSpeed - (0.5f * (customerId - 1));

            // Later customer stand behind the earlier customer/s
            var NewQueuePosition = new Vector3(queuePosition.x, queuePosition.y, queuePosition.z + (customerSizeX * (customerId - 1)));

            targetPosition = NewQueuePosition;
            dir = (NewQueuePosition - transform.position).normalized;
        }
    }

    // Order food based on parameter
    public void OrderFood(FoodSO food)
    {
        // Show Food Bubble Image
        GameObject canvas = transform.GetChild(1).gameObject;
        canvas.SetActive(true);
        canvas.transform.GetChild(1).GetComponent<Image>().sprite = food.sprite;
        canvas.transform.GetChild(1).GetComponent<Image>().preserveAspect = true;

        // Spawn Tableware
        if(food.foodName.Equals("Otah"))
        {
            var obj = ObjectPool.Instance.GetPooledObject(plate);

            if (!obj) return;
            
            obj.transform.parent = transform;
            SetTableWareData();
        }
        else
        {
            var obj = ObjectPool.Instance.GetPooledObject(bowl);

            if (!obj) return;
            
            obj.transform.parent = transform;
            SetTableWareData();
        }

        // Set Ordered Food
        foodOrdered = food;
        orderedFood = true;
    }

    // Set customer gameobject to inactive to simulate GameObject.Destory()
    public void Destroy()
    {
        gameObject.SetActive(false);
        orderedFood = false;
        reachedTarget = false;
        foodOrdered = null;
    }

    // Leave the store
    public void Leave(int customerId)
    {
        // Remove Customer with stated customerId
        CustomerSpawner.Instance.customerCount -= 1;
        CustomerSpawner.Instance.customerDic[customerId].Destroy();
        CustomerSpawner.Instance.customerDic.Remove(customerId);

        // Only change customerId when Serving customer 1 or 2
        if (customerId == 1 || customerId == 2)
        {
            var newDic = new Dictionary<int, Customer>();

            switch (customerId)
            {
                case 1:
                    {
                        int newKey;
                        foreach (var item in CustomerSpawner.Instance.customerDic)
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
                        foreach (var item in CustomerSpawner.Instance.customerDic)
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

            // Check if still have customer
            if (newDic.Count == 0)
            {
                // No more customer
                Menu_Manager.Instance.OnGameMenu();
            }
            else
            {
                // Copy newDic into our customerDic
                CustomerSpawner.Instance.customerDic = newDic;
            }
        }
    }

    private void Update ()
    {
        // Customer Waiting for food
        if(orderedFood)
        {
            // Reduce fillAmount of Timer Filler Image(visual feedback) over waitTiming
            timerImage.fillAmount -= (1f / waitTiming) * Time.deltaTime;

            // Left more than half the time -> Image turning from green to yellow
            if(timerImage.fillAmount >= 0.5f)
            {
                timerImage.color = greenYellowGradient.Evaluate(timerImage.fillAmount / 1f);
            }
            // Left less than half the time -> Image turning from yellow to red
            else if(timerImage.fillAmount > 0f)
            {
                timerImage.color = yellowRedGradient.Evaluate(timerImage.fillAmount / 1f);
            }
            else if(timerImage.fillAmount <= 0f)
            {
                // Leave
                Leave(customerId);
            }
        }

        // Move towards the Target position
        if(Vector3.Distance(transform.position, targetPosition) >= 0.1f)
        {
            // Rotate to face Wall
            if (orderedFood && Vector3.Angle(transform.forward, new Vector3(1, 0, 0) ) != 90f)
            {
                var tableWare = transform.GetChild(3).gameObject;
                if (tableWare.activeInHierarchy)
                {
                    tableWare.SetActive(false);
                }

                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z), rotateSpeed * Time.deltaTime);
            }
            else
            {
                transform.position += dir * movementSpeed * Time.deltaTime;
            }
        }
        else
        {
            reachedTarget = true;

            //Debug.Log(Vector3.Angle(transform.forward, new Vector3(1, 0, 0)));
            // Rotate to face Player(Chef)
            if (Vector3.Angle(transform.forward, new Vector3(1, 0, 0)) != 180f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, -90f, transform.rotation.z), rotateSpeed * Time.deltaTime);

                if (Vector3.Angle(transform.forward, new Vector3(1, 0, 0)) == 180f)
                {
                    if (!orderedFood)
                    {
                        OrderFood(foodOrder[Random.Range(0, foodOrder.Length)]);
                    }
                    ////////////transform.GetChild(3).gameObject.SetActive(true);
                    ////////////SetTableWareData();
                }
            }
        }
	}
}