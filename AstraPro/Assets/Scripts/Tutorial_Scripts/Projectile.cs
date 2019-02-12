using UnityEngine;

/// <summary>
/// This class manages movement and active status of Projectile.
/// 
/// Can be found attached in each Projectile Prefab.
/// </summary>
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// A float that determines how much the projectile is going to rotate.
    /// </summary>
    [Tooltip("A float that determines how much the projectile is going to rotate.")]
    [SerializeField] private float torque;

    /// <summary>
    /// Used for rotation and collision against player's collider component.
    /// </summary>
    private Rigidbody rigidBody;

    /// <summary>
    /// A Vector3 that determines where this projectile move towards.
    /// </summary>
    [HideInInspector]
    public Vector3 dir;

    private void OnEnable()
    {
        rigidBody = GetComponent<Rigidbody>();
        
        // Disable itself with the Destroy() below, after 2 seconds of being active
        Invoke("Destroy", 2f);
    }

    /// <summary>
    /// Deactivates this GameObject for later use through Object Pooling.
    /// </summary>
    public void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        // Prevent calling Destroy() again through Invoke() when this GameObject is already inactive.
        CancelInvoke();
    }

    private void Update()
    {
        // If the game is in Pause Status, do not do anything
        if (PauseManager.Instance != null && PauseManager.Instance.isPaused)
        {
            return;
        }

        // Travel according to the Dir(Direction) set when spawn
        transform.position += dir * 5 * Time.deltaTime;

        // Using Torque to rotate the Projectiles
        rigidBody.AddRelativeTorque(Vector3.forward * torque);
        rigidBody.AddRelativeTorque(Vector3.up * torque);
        rigidBody.AddRelativeTorque(Vector3.right * torque);
    }
}