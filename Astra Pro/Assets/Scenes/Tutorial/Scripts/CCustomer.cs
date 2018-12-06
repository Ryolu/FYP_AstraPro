using UnityEngine;

public class CCustomer : MonoBehaviour
{
    [Tooltip("Movement Speed of Customer")] public float m_fMovementSpeed = 10f;
    [HideInInspector] public Vector3 m_QueuePosition;
    [HideInInspector] public int m_iCustomerCount;

    private float m_fCustomerSize;
    private Vector3 m_Dir;
    private Vector3 m_TargetPosition;

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
            m_Dir = (m_QueuePosition - transform.position).normalized;

            m_TargetPosition = m_QueuePosition;
        }
        else
        {
            Vector3 NewQueuePosition = new Vector3(m_QueuePosition.x - (m_fCustomerSize * (m_iCustomerCount - 1)), m_QueuePosition.y, m_QueuePosition.z);

            m_TargetPosition = NewQueuePosition;

            m_Dir = (NewQueuePosition - transform.position).normalized;
        }
    }

    // Move towards the Target position
	void Update ()
    {
        if(Vector3.Distance(transform.position, m_TargetPosition) >= 1f)
            transform.position += m_Dir * m_fMovementSpeed * Time.deltaTime;
	}
}
