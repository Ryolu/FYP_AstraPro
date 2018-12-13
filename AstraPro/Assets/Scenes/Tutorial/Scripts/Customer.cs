using UnityEngine;

public class Customer : MonoBehaviour
{
    [Tooltip("Movement Speed of Customer")] public float movementSpeed = 10f;

    [HideInInspector] public Vector3 queuePosition;
    [HideInInspector] public int customerCount;
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
    private void CalculateDir()
    {
        if(customerCount == 1)
        {
            targetPosition = queuePosition;
            dir = (queuePosition - transform.position).normalized;
        }
        else
        {
            // Later customer walk slower
            movementSpeed = movementSpeed - (4f * (customerCount - 1));

            // Later customer stand behind the earlier customer/s
            var NewQueuePosition = new Vector3(queuePosition.x - (customerSizeX * (customerCount - 1)), queuePosition.y, queuePosition.z);

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
        }
	}
}
