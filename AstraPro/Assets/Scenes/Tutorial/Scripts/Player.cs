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
        if (other.gameObject.tag == "Lipstick" || other.gameObject.tag == "Phone" ||
            other.gameObject.tag == "Purse" || other.gameObject.tag == "Slipper")
        {
            Score.Instance.rate -= 0.1f;
            Score.Instance.fillimage.GetComponent<Animation>().Play();
            other.GetComponent<Projectile>().Destroy();
        }
    }
}
