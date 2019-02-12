using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// This class manages everything of Customer AI.
/// 
/// Can be found attached in Assets -> Prefabs -> Customers
/// </summary>
public class Customer : MonoBehaviour
{
    /// <summary>
    /// Enumeration to determine Leaving States of Customer.
    /// </summary>
    private enum LeavingStates
    {
        phase1,
        phase2,
        phase3
    }

    public object LeavingState { get; private set; }

    /// <summary>
    /// Enumeration to determine Fight Positions.
    /// </summary>
    private enum FightPositions
    {
        left = 1,
        middle = 2,
        right = 3
    }
    
    /// <summary>
    /// Image Component of Rotating Timer(Color)
    /// </summary>
    [Tooltip("Image Component of Rotating Timer(Color)")]
    public Image timerImage;

    /// <summary>
    /// A float that determines how long the customer will wait for food.
    /// 
    /// Default: 30
    /// </summary>
    [Tooltip("A float that determines how long the customer will wait for food.\n" +
        "\n" +
        "Default: 30")]
    public float waitTiming = 30f;

    /// <summary>
    /// A float that determines Movement Speed of Customer.
    /// 
    /// Default: 2.5
    /// </summary>
    [Tooltip("A float that determines Movement Speed of Customer.\n" +
        "\n" +
        "Default: 2.5")]
    [SerializeField] private float movementSpeed = 2.5f;

    /// <summary>
    /// A float that determines Rotate Speed of Customer.
    /// 
    /// Default: 80
    /// </summary>
    [Tooltip("A float that determines Rotate Speed of Customer.\n" +
        "\n" +
        "Default: 80")]
    [SerializeField] private float rotateSpeed = 80f;

    /// <summary>
    /// A float that determines Cool down timing of shooting player.
    /// 
    /// Default: 1
    /// </summary>
    [Tooltip("A float that determines Cool down timing of shooting player.\n" +
        "\n" +
        "Default: 1")]
    [SerializeField] private float fireCD = 1f;

    /// <summary>
    /// An array of Foods that customer can order.
    /// </summary>
    [Tooltip("An array of Foods that customer can order.")]
    [SerializeField] private FoodSO[] foodOrder;

    /// <summary>
    /// A GameObject Prefab of Customer Bullet.
    /// </summary>
    [Tooltip("Customer Bullet Prefab to shoot player")]
    [SerializeField] private GameObject customerBulletPrefab;

    /// <summary>
    /// A Vector3 Postion for Customer to queue towards.
    /// </summary>
    [HideInInspector] public Vector3 queuePosition;

    /// <summary>
    /// An integer that stores customer's ID.
    /// </summary>
    [HideInInspector] public int customerId;

    /// <summary>
    /// A boolean that determines if Customer have reach their target as they move.
    /// 
    /// Defualt: False
    /// </summary>
    [HideInInspector] public bool reachedTarget = false;

    /// <summary>
    /// A boolean that determines if Customer have order food.
    /// 
    /// Defualt: False
    /// </summary>
    [HideInInspector] public bool orderedFood = false;

    /// <summary>
    /// A reference to store the food ordered by customer.
    /// </summary>
    [HideInInspector] public FoodSO foodOrdered;

    /// <summary>
    /// A boolean that determines if Customer is fighting.
    /// 
    /// Defualt: False
    /// </summary>
    [HideInInspector] public bool fighting = false;

    /// <summary>
    /// A boolean that determines if other Customer is fighting.
    /// 
    /// Defualt: False
    /// </summary>
    [HideInInspector] public bool othersFighting = false;

    /// <summary>
    /// A boolean that determines if Customer is leaving.
    /// 
    /// Defualt: False
    /// </summary>
    [HideInInspector] public bool leaving = false;

    /// <summary>
    /// A reference to the Transform of Player.
    /// </summary>
    [HideInInspector] public Transform player;

    /// <summary>
    /// A String reference to the Customer State.
    /// 
    /// Default: IsIdle
    /// </summary>
    [HideInInspector] public string idle = "IsIdle";

    /// <summary>
    /// A String reference to the Customer State.
    /// 
    /// Default: IsWalking
    /// </summary>
    [HideInInspector] public string walking = "IsWalking";

    /// <summary>
    /// A String reference to the Customer State.
    /// 
    /// Default: IsHappy
    /// </summary>
    [HideInInspector] public string happy = "IsHappy";

    /// <summary>
    /// A String reference to the Customer State.
    /// 
    /// Default: IsAngry
    /// </summary>
    [HideInInspector] public string angry = "IsAngry";

    /// <summary>
    /// A String reference to the Customer State.
    /// 
    /// Default: IsScared
    /// </summary>
    [HideInInspector] public string scared = "IsScared";

    /// <summary>
    /// A String reference to the Customer State.
    /// 
    /// Default: IsThrowingStuff
    /// </summary>
    [HideInInspector] public string throwing = "IsThrowingStuff";

    /// <summary>
    /// A Vector3 that determines where this customer move towards.
    /// </summary>
    private Vector3 dir;

    /// <summary>
    /// A Vector3 Postion for Customer to move towards.
    /// </summary>
    private Vector3 targetPosition;
    
    /// <summary>
    /// A Vector3 Postion for Customer to leave towards.
    /// </summary>
    private Vector3 leavingPosition;

    /// <summary>
    /// A float that determines the customer size x.
    /// </summary>
    private float customerSizeX;

    /// <summary>
    /// A float that determines the customer size z.
    /// </summary>
    private float customerSizeZ;

    /// <summary>
    /// A gradient that determines the colour of customer order image.
    /// </summary>
    private Gradient greenYellowGradient;

    /// <summary>
    /// A gradient that determines the colour of customer order image.
    /// </summary>
    private Gradient yellowRedGradient;

    /// <summary>
    /// A float timer of customer throwing stuff.
    /// </summary>
    private float timer = 0f;

    /// <summary>
    /// A GameObject reference to the clock Hand Image of Customer Order Image.
    /// </summary>
    private GameObject clockHand;

    /// <summary>
    /// A Animator reference to cusotmer's animator.
    /// </summary>
    private Animator anim;

    /// <summary>
    /// A AudioSource reference to customer's audio source.
    /// </summary>
    private AudioSource audio;

    /// <summary>
    /// A HandPointer reference to player's left hand.
    /// </summary>
    private HandPointer playerLHand;

    /// <summary>
    /// A HandPointer reference to player's right hand.
    /// </summary>
    private HandPointer playerRHand;

    /// <summary>
    /// Enumeration to determine Leaving States of Customer.
    /// </summary>
    private LeavingStates leavingState;

    /// <summary>
    /// Enumeration to determine Fight Positions.
    /// </summary>
    private FightPositions fightPositions;

    /// <summary>
    /// A boolean that determine if Customer should dodge.
    /// </summary>
    private bool dodge = false;
    
    /// <summary>
    /// Initialise Data for customer.
    /// </summary>
    public void InitiateData()
    {
        customerSizeX = transform.lossyScale.x * 0.275f;
        customerSizeZ = transform.lossyScale.z * 0.275f;
        waitTiming = Random.Range((waitTiming / 2), waitTiming);
        anim = GetComponent<Animator>();
        SetAnim(idle, false);
        SetAnim(walking, true);
        leavingState = LeavingStates.phase1;
        fightPositions = (FightPositions)customerId;
        dodge = false;
        audio = GetComponent<AudioSource>();

        var spawnPos = CustomerSpawner.Instance.spawnPoint.position;
        leavingPosition = new Vector3(spawnPos.x + customerSizeX * 0.75f, spawnPos.y, spawnPos.z + customerSizeZ);

        var hands = PauseManager.Instance.GetComponentsInChildren<HandPointer>();
        foreach(var hand in hands)
        {
            if (hand.currentHand == HandPointer.Hands.left)
            {
                playerLHand = hand;
            }
            else if(hand.currentHand == HandPointer.Hands.right)
            {
                playerRHand = hand;
            }
        }

        CalculateDir();
        InitiateColor();
        AllowHover(false);
    }
    
    /// <summary>
    /// Set Animation to play for customer with passed in variables.
    /// </summary>
    /// <param name="state"> States of the animation. </param>
    /// <param name="status"> True or False to determine playing the animation or not. </param>
    public void SetAnim(string state, bool status)
    {
        anim.SetBool(state, status);
    }

    /// <summary>
    /// Set the Audio Clip of customer's audio source, stop previous clip and start playing current clip.
    /// </summary>
    /// <param name="audioClip"></param>
    public void SetClip(AudioClip audioClip)
    {
        audio.Stop();
        audio.clip = audioClip;
        audio.Play();
    }

    /// <summary>
    /// Initiate Gradients, which is used to change color based on fillAmount of timerImage.
    /// </summary>
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

    /// <summary>
    /// Calculate Direction for customer to move and Record down the Target position based on number of customer.
    /// </summary>
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

    /// <summary>
    /// Order food based on parameter.
    /// </summary>
    /// <param name="food"> The food to order. </param>
    public void OrderFood(FoodSO food)
    {
        // Show Food Bubble Image
        GameObject canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(true);
        clockHand = canvas.transform.GetChild(3).gameObject;
        canvas.transform.GetChild(4).GetComponent<Image>().sprite = food.sprite;
        canvas.transform.GetChild(4).GetComponent<Image>().preserveAspect = true;

        // Set Ordered Food
        foodOrdered = food;
        orderedFood = true;
    }

    /// <summary>
    /// Turn clock hand image in Food Order canvas based on timer fill amount.
    /// </summary>
    /// <param name="percentage"> Percentage of current timer of waiting. </param>
    private void TurnClockHand(float percentage)
    {
        var angle = percentage * 360f;
        clockHand.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    /// <summary>
    /// Deactivates this GameObject and Reset its data for later use through The Object Pool.
    /// </summary>
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

    /// <summary>
    /// Leave the store.
    /// </summary>
    public void Leave()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        leaving = true;

        leavingState = LeavingStates.phase1;
    }

    /// <summary>
    /// Allow interaction with customer.
    /// </summary>
    /// <param name="allow"> True or False to determine should allow or not. </param>
    public void AllowHover(bool allow)
    {
        transform.GetChild(1).gameObject.SetActive(allow);
    }

    /// <summary>
    /// Remove customer from Dictionary and Make the other active customer to move up the queue.
    /// </summary>
    /// <param name="customerId"> The Customer ID to remove. </param>
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
        // If the game is in Pause Status, do not do anything
        if (PauseManager.Instance != null && PauseManager.Instance.isPaused)
        {
            return;
        }

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

                                if(Menu_Manager.Instance.Tutorial_Mode && !Guide.Instance.gameObject.activeSelf)
                                {
                                    Guide.Instance.Show();
                                }
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

                        foreach (CookingAppliance appliance in LevelManager.Instance.cookingAppliances)
                            if (!appliance.isDone)
                                appliance.NewFood();

                        player = Player.Instance.transform;

                        if (Guide.Instance != null)
                        {
                            Guide.Instance.gameObject.SetActive(true);
                        }

                        SetAnim(idle, false);
                        SetAnim(angry, true);

                        playerLHand.DropItem(null);
                        playerRHand.DropItem(null);
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

            if (Vector3.Distance(transform.position, leavingPosition) >= 0.1f)
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
                        if (Vector3.Angle(transform.forward, leavingPosition - transform.position) != 0f)
                        {
                            Vector3 temp = leavingPosition - transform.position;
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

    /// <summary>
    /// This makes this customer leave when gets hit by Player's Projectile.
    /// And destroy the projectiles by calling Destroy() in Projectile Script.
    /// </summary>
    /// <param name="other"> The collider component in the Projectile. </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Knife")
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