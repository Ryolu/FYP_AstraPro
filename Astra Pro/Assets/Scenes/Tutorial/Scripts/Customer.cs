using UnityEngine;

public class Customer : MonoBehaviour
{
    [Tooltip("Movement Speed of Customer")] public float movementSpeed = 10f;

    [HideInInspector] public Vector3 queuePosition;
    [HideInInspector] public int customerId;
    [HideInInspector] public bool reachedTarget = false;
    [HideInInspector] public bool orderedFood = false;
    
    private Vector3 dir;
    private Vector3 targetPosition;
    private float customerSizeX;

	private void Start ()
    {
        customerSizeX = transform.lossyScale.x * 1.5f;

        CalculateDir();
	}
	
    // Calculate Direction for customer to move and Record down the Target position based on number of customer
    public void CalculateDir()
    {
        // Force to false, to allow movement towards target
        if(reachedTarget && movementSpeed < 10f)
        {
            reachedTarget = false;
            movementSpeed = 10f;
        }

        if(customerId == 1)
        {
            targetPosition = queuePosition;
            dir = (queuePosition - transform.position).normalized;
        }
        else
        {
            // Later customer walk slower
            movementSpeed = movementSpeed - (4f * (customerId - 1));

            // Later customer stand behind the earlier customer/s
            var NewQueuePosition = new Vector3(queuePosition.x, queuePosition.y, queuePosition.z + (customerSizeX * (customerId - 1)));

            targetPosition = NewQueuePosition;
            dir = (NewQueuePosition - transform.position).normalized;
        }
    }

    public void OrderFood()
    {
        // Show Food Bubble Image
        transform.GetChild(0).gameObject.SetActive(true);
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
            transform.position += dir * movementSpeed * Time.deltaTime;
        }
        else
        {
            reachedTarget = true;
            transform.Rotate(Vector3.up, -90f);
            //transform.rotat = Vector3.Lerp(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z), new Vector3(transform.eulerAngles.x,-90f,transform.eulerAngles.z), movementSpeed * Time.deltaTime);
        }
	}
}
