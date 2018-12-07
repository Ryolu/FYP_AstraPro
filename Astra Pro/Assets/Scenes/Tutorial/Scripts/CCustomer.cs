using UnityEngine;

public class CCustomer : MonoBehaviour
{
    [Tooltip("Movement Speed of Customer")] public float m_fMovementSpeed = 10f;

    [HideInInspector] public Vector3 m_QueuePosition;
    [HideInInspector] public int m_iCustomerCount;
    [HideInInspector] public bool m_bReachedTarget = false;
    [HideInInspector] public bool m_bOrderedFood = false;
    
    private Vector3 m_Dir;
    private Vector3 m_TargetPosition;
    private float m_fCustomerSize;

	private void Start ()
    {
        m_fCustomerSize = transform.lossyScale.x * 1.5f;

        CalculateDir();
	}
	
    // Calculate Direction for customer to move and Record down the Target position based on number of customer
    private void CalculateDir()
    {
        if(m_iCustomerCount == 1)
        {
            m_TargetPosition = m_QueuePosition;
            m_Dir = (m_QueuePosition - transform.position).normalized;
        }
        else
        {
            // Later customer walk slower
            m_fMovementSpeed = m_fMovementSpeed - (4f * (m_iCustomerCount - 1));

            // Later customer stand behind the earlier customer/s
            Vector3 NewQueuePosition = new Vector3(m_QueuePosition.x - (m_fCustomerSize * (m_iCustomerCount - 1)), m_QueuePosition.y, m_QueuePosition.z);

            m_TargetPosition = NewQueuePosition;
            m_Dir = (NewQueuePosition - transform.position).normalized;
        }
    }

    public void OrderFood()
    {
        // Show Food Bubble Image
        transform.GetChild(0).gameObject.SetActive(true);
        m_bOrderedFood = true;
    }

	private void Update ()
    {
        // Move towards the Target position
        if(!m_bReachedTarget && Vector3.Distance(transform.position, m_TargetPosition) >= 1f)
            transform.position += m_Dir * m_fMovementSpeed * Time.deltaTime;
        else
            m_bReachedTarget = true;
	}
}
