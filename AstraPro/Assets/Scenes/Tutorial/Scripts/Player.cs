using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private void Awake()
    {
        // Set instance for other Scripts to access
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CustomerBullet")
        {
            Score.Instance.rate -= 0.1f;
            other.GetComponent<Projectile>().Destroy();
        }
    }
}
