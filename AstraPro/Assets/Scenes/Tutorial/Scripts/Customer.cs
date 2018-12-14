using UnityEngine;

public class Customer : MonoBehaviour
{
    [Tooltip("Movement Speed of Customer")] public float movementSpeed = 25f;
    [Tooltip("Movement Speed of Customer")] public float rotateSpeed = 80f;

    [HideInInspector] public Vector3 queuePosition;
    [HideInInspector] public int customerId;
    [HideInInspector] public bool reachedTarget = false;
    [HideInInspector] public bool orderedFood = false;
    
    private Vector3 dir;
    private Vector3 targetPosition;
    private float customerSizeX;

	private void Start ()
    {
        customerSizeX = transform.lossyScale.x * 25f;
        CalculateDir();
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

    public void OrderFood()
    {
        // Show Food Bubble Image
        transform.Find("Canvas").gameObject.SetActive(true);
        orderedFood = true;
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void Update ()
    {
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
            if(transform.eulerAngles.y >= -90f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, -90f,transform.rotation.z), rotateSpeed * Time.deltaTime);
            }
        }
	}
}
