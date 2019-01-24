using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float torque;
    [HideInInspector] public Vector3 dir;

    private Rigidbody rigidbody;

    // Disable itself after 2 seconds of being active
    private void OnEnable()
    {
        Invoke("Destroy", 2f);
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    // Travel according to the Dir(Direction) stated when spawn
    private void Update()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.isPaused) return;

        transform.position += dir * 5 * Time.deltaTime;
        rigidbody.AddRelativeTorque(Vector3.forward * torque);
        //transform.rotation = new Quaternion(0, Time.deltaTime, 0, 0); 
    }
}
