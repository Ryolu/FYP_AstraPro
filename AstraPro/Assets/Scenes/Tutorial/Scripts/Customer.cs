using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [Tooltip("Movement Speed of Customer")] [SerializeField] private float movementSpeed = 25f;
    [Tooltip("Rotate Speed of Customer")] [SerializeField] private float rotateSpeed = 80f;
    [Tooltip("How long the Customer will wait for food")] [SerializeField] private float waitTiming = 30f;
    [Tooltip("Timer Filler Image")] [SerializeField] private Image timerImage;
    [Tooltip("Order Images")] [SerializeField] private Sprite[] orderSprites;

    [HideInInspector] public Vector3 queuePosition;
    [HideInInspector] public int customerId;
    [HideInInspector] public bool reachedTarget = false;
    [HideInInspector] public bool orderedFood = false;
    [HideInInspector] public enum FoodOrder { Otah = 1, FishHeadCurry = 2, MeeSiam = 3, Laksa = 4};

    private Vector3 dir;
    private Vector3 targetPosition;
    private float customerSizeX;
    private Gradient greenYellowGradient;
    private Gradient yellowRedGradient;


    private void Start ()
    {
        customerSizeX = transform.lossyScale.x * 25f;
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

    // Calculate Direction for customer to move and Record down the Target position based on number of customer
    public void CalculateDir()
    {
        // Force to false, to allow movement towards target
        if(reachedTarget && movementSpeed < 25f)
        {
            reachedTarget = false;
            movementSpeed = 25f;
        }

        if(customerId == 1)
        {
            targetPosition = queuePosition;
            dir = (queuePosition - transform.position).normalized;
        }
        else
        {
            // Later customer walk slower
            movementSpeed = movementSpeed - (6.5f * (customerId - 1));

            // Later customer stand behind the earlier customer/s
            var NewQueuePosition = new Vector3(queuePosition.x, queuePosition.y, queuePosition.z + (customerSizeX * (customerId - 1)));

            targetPosition = NewQueuePosition;
            dir = (NewQueuePosition - transform.position).normalized;
        }
    }

    public void OrderFood(FoodOrder foodOrder)
    {
        // Show Food Bubble Image
        GameObject canvas = transform.GetChild(1).gameObject;
        canvas.SetActive(true);
        canvas.transform.GetChild(1).GetComponent<Image>().sprite = orderSprites[(int)foodOrder - 1];
        canvas.transform.GetChild(1).GetComponent<Image>().preserveAspect = true;
        orderedFood = true;
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
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
                CustomerSpawner.Instance.Leave(customerId);
            }
        }

        // Move towards the Target position
        if(!reachedTarget && Vector3.Distance(transform.position, targetPosition) >= 1f)
        {
            // Rotate to face Wall
            if (orderedFood && transform.eulerAngles.y > 180f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z), rotateSpeed * Time.deltaTime);
                return;
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
            if(transform.eulerAngles.y > -90f)
            {
                if (transform.eulerAngles.y >= 269f && !orderedFood)
                {
                    OrderFood((FoodOrder)Random.Range(1, 5));
                }

                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, -90f,transform.rotation.z), rotateSpeed * Time.deltaTime);
            }
            
        }
	}
}
