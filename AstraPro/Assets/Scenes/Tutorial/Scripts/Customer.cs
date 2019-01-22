using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Customer : MonoBehaviour
{
    enum LeavingStates
    {
        phase1,
        phase2,
        phase3
    }

    LeavingStates leavingState;

    enum FightPositions
    {
        left = 1,
        middle = 2,
        right = 3
    }

    FightPositions fightPositions;
    private bool dodge = false;

    [Tooltip("Timer Filler Image")] public Image timerImage;
    [Tooltip("Movement Speed of Customer")] [SerializeField] private float movementSpeed = 2.5f;
    [Tooltip("Rotate Speed of Customer")] [SerializeField] private float rotateSpeed = 80f;
    [Tooltip("How long the Customer will wait for food")] [SerializeField] private float waitTiming = 30f;
    [Tooltip("Cool down timing of shooting player")] [SerializeField] private float fireCD = 1f;
    [Tooltip("Foods that customer can order")] [SerializeField] private FoodSO[] foodOrder;
    [Tooltip("Customer Bullet Prefab to shoot player")] [SerializeField] private GameObject customerBulletPrefab;

    [HideInInspector] public Vector3 queuePosition;
    [HideInInspector] public int customerId;
    [HideInInspector] public bool reachedTarget = false;
    [HideInInspector] public bool orderedFood = false;
    [HideInInspector] public FoodSO foodOrdered;
    [HideInInspector] public bool fighting = false;
    [HideInInspector] public bool othersFighting = false;
    [HideInInspector] public bool leaving = false;
    [HideInInspector] public Transform player;

    private Vector3 dir;
    private Vector3 targetPosition;
    private float customerSizeX;
    private float customerSizeZ;
    private Gradient greenYellowGradient;
    private Gradient yellowRedGradient;
    private float timer = 0f;
    private GameObject clockHand;
    private Animator anim;
    [HideInInspector] public string idle = "IsIdle";
    [HideInInspector] public string walking = "IsWalking";
    [HideInInspector] public string happy = "IsHappy";
    [HideInInspector] public string angry = "IsAngry";
    [HideInInspector] public string scared = "IsScared";
    [HideInInspector] public string throwing = "IsThrowingStuff";

    public object LeavingState { get; private set; }

    public void InitiateData()
    {
        customerSizeX = transform.lossyScale.x * 0.275f;
        customerSizeZ = transform.lossyScale.z * 0.275f;
        waitTiming = Random.Range((waitTiming / 3), waitTiming);
        anim = GetComponent<Animator>();
        SetAnim(idle, false);
        SetAnim(walking, true);
        leavingState = LeavingStates.phase1;
        fightPositions = (FightPositions)customerId;
        dodge = false;

        CalculateDir();
        InitiateColor();
    }
    
    public void SetAnim(string state, bool status)
    {
        anim.SetBool(state, status);
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
        GameObject canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(true);
        clockHand = canvas.transform.GetChild(2).gameObject;
        canvas.transform.GetChild(3).GetComponent<Image>().sprite = food.sprite;
        canvas.transform.GetChild(3).GetComponent<Image>().preserveAspect = true;

        // Set Ordered Food
        foodOrdered = food;
        orderedFood = true;
    }

    // Turn clock hand image in Food Order canvas based on timer fill amount
    private void TurnClockHand(float percentage)
    {
        var angle = percentage * 360f;
        clockHand.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // Set customer gameobject to inactive to simulate GameObject.Destory()
    public void Destroy()
    {
        gameObject.SetActive(false);

        timerImage.fillAmount = 1f;
        movementSpeed = 2.5f;
        reachedTarget = false;
        orderedFood = false;
        foodOrdered = null;
        fighting = false;
        othersFighting = false;
        leaving = false;
        player = null;
        timer = 0f;
        clockHand = null;
        leavingState = LeavingStates.phase1;
        fightPositions = (FightPositions)customerId;
        dodge = false;
    }

    // Leave the store
    public void Leave()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        targetPosition = CustomerSpawner.Instance.spawnPoint.position + new Vector3(customerSizeX * 0.75f, 0, customerSizeZ);
        dir = (targetPosition - transform.position).normalized;
        leaving = true;

        leavingState = LeavingStates.phase1;
    }

    public void RemoveCustomer(int customerId)
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
            if (Menu_Manager.Instance.Tutorial_Mode && newDic.Count == 0)
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

    private void Update()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.isPaused) return;

        // Debug Key => 1st customer leaves
        if(Input.GetKeyDown(KeyCode.L))
        {
            if(customerId == 1)
            {
                SetAnim(idle, false);
                SetAnim(happy, true);
                Leave();
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (customerId == 1)
            {
                SetAnim(idle, false);
                SetAnim(scared, true);
                Leave();
            }
        }


        if (!leaving)
        {
            if (!fighting)
            {
                #region Move & Order State
                // Move towards the Target position
                if (Vector3.Distance(transform.position, targetPosition) >= 0.1f)
                {
                    SetAnim(idle, false);
                    SetAnim(walking, true);

                    // Rotate to face Wall
                    if (orderedFood && Vector3.Angle(transform.forward, new Vector3(1, 0, 0)) != 90f)
                    {
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

                    // Rotate to face Player(Chef)
                    if (Vector3.Angle(transform.forward, new Vector3(1, 0, 0)) != 180f)
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, -90f, transform.rotation.z), rotateSpeed * Time.deltaTime);

                        if (Vector3.Angle(transform.forward, new Vector3(1, 0, 0)) == 180f)
                        {
                            SetAnim(walking, false);
                            SetAnim(idle, true);

                            if (!orderedFood)
                            {
                                OrderFood(foodOrder[Random.Range(0, foodOrder.Length)]);
                            }
                        }
                    }
                }
                #endregion Move & Order State

                #region Waiting State
                if (orderedFood)
                {
                    // Reduce fillAmount of Timer Filler Image(visual feedback) over waitTiming
                    timerImage.fillAmount -= (1f / waitTiming) * Time.deltaTime;
                    TurnClockHand(timerImage.fillAmount);

                    // Left more than half the time -> Image turning from green to yellow
                    if (timerImage.fillAmount >= 0.5f)
                    {
                        timerImage.color = greenYellowGradient.Evaluate(timerImage.fillAmount / 1f);
                    }
                    // Left less than half the time -> Image turning from yellow to red
                    else if (timerImage.fillAmount > 0f)
                    {
                        timerImage.color = yellowRedGradient.Evaluate(timerImage.fillAmount / 1f);
                    }
                    else if (timerImage.fillAmount <= 0f)
                    {
                        fighting = true;
                        player = Player.Instance.transform;

                        if (Guide.Instance != null)
                        {
                            Guide.Instance.gameObject.SetActive(true);
                        }

                        SetAnim(idle, false);
                        SetAnim(angry, true);
                    }
                }
                #endregion Waiting State
            }
            else
            {
                if (dodge)
                {
                    #region Dodge State
                    if (Vector3.Distance(transform.position, targetPosition) >= 0.1f)
                    {
                        transform.position += dir * (movementSpeed * movementSpeed) * Time.deltaTime;
                    }
                    else
                    {
                        dodge = false;
                    }
                    #endregion Dodge State
                }
                else
                {
                    #region Fighting State
                    // Hide Food Bubble Image
                    if (transform.GetChild(0).gameObject.activeSelf)
                        transform.GetChild(0).gameObject.SetActive(false);

                    timer += Time.deltaTime;
                    if (timer >= fireCD)
                    {
                        if (player)
                        {
                            // Shoot bullet towards hand icon
                            var Projectile = ObjectPool.Instance.GetPooledObject(customerBulletPrefab);

                            if (!Projectile) return;

                            var a = Random.Range(0.01f, 1f);
                            var randPos = new Vector3(Random.Range(player.position.x - a, player.position.x + a),
                                Random.Range(player.position.y - a, player.position.y + a),
                                Random.Range(player.position.z - a, player.position.z + a));

                            Projectile.transform.position = transform.GetChild(2).position;
                            Projectile.transform.rotation = Quaternion.identity;
                            Projectile.GetComponent<Projectile>().dir = (randPos - Projectile.transform.position).normalized;

                            // Scare off other customers
                            foreach (var pair in CustomerSpawner.Instance.customerDic)
                            {
                                if (pair.Value.customerId != customerId)
                                {
                                    if (!pair.Value.othersFighting)
                                    {
                                        pair.Value.othersFighting = true;
                                    }
                                    else
                                    {
                                        if (!pair.Value.leaving)
                                        {
                                            pair.Value.SetAnim(idle, false);
                                            pair.Value.SetAnim(angry, false);
                                            pair.Value.SetAnim(scared, true);
                                            pair.Value.Leave();
                                        }
                                    }
                                }
                            }

                            #region Dodge Decision State
                            var rand = Random.Range(1, 4);
                            if (rand == customerId || (FightPositions)rand == fightPositions)
                            {
                                dodge = false;
                            }
                            else
                            {
                                dodge = true;
                                fightPositions = (FightPositions)rand;

                                switch (fightPositions)
                                {
                                    case FightPositions.left:
                                        {
                                            targetPosition = new Vector3(queuePosition.x, queuePosition.y, queuePosition.z + (customerSizeX * 2));
                                        }
                                        break;
                                    case FightPositions.middle:
                                        {
                                            targetPosition = new Vector3(queuePosition.x, queuePosition.y, queuePosition.z + customerSizeX);
                                        }
                                        break;
                                    case FightPositions.right:
                                        {
                                            targetPosition = queuePosition;
                                        }
                                        break;
                                }
                                dir = (targetPosition - transform.position).normalized;
                            }
                            #endregion Dodge Decision State
                        }
                        timer = 0f;
                    }
                    #endregion Fighting State
                }
            }
        }
        else
        {
            #region Leaving State

            if (Vector3.Distance(transform.position, targetPosition) >= 0.1f)
            {
                switch (leavingState)
                {
                    case LeavingStates.phase1:
                        if (Vector3.Angle(transform.forward, new Vector3(1, 0, 0)) > 2.0f)
                        {
                            transform.forward = Vector3.RotateTowards(transform.forward, new Vector3(1, 0, 0), 0.05f, 0.0f);
                        }
                        else
                        {
                            leavingState = LeavingStates.phase2;
                        }
                        break;

                    case LeavingStates.phase2:
                        if (Vector3.Angle(transform.forward, targetPosition - transform.position) != 0f)
                        {
                            Vector3 temp = targetPosition - transform.position;
                            transform.forward = Vector3.RotateTowards(transform.forward, new Vector3(temp.x, 0, temp.z), 0.075f, 0.0f);
                            transform.position += transform.forward * Time.deltaTime * 2f;
                        }
                        else
                        {
                            leavingState = LeavingStates.phase3;
                        }
                        break;

                    case LeavingStates.phase3:
                        {
                            transform.position += transform.forward * Time.deltaTime * 2f;
                        }
                        break;
                }
            }
            else
            {
                RemoveCustomer(customerId);
            }

            #endregion Leaving State
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            if (fighting)
            {
                SetAnim(throwing, false);
                SetAnim(scared, true);
                Leave();
                other.transform.GetComponent<Projectile>().Destroy();
            }
        }
    }
}