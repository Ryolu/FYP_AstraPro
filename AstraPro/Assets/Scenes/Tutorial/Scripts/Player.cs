using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "CustomerBullet")
        {
            Score.Instance.rate -= 0.1f;
            other.GetComponent<Projectile>().Destroy();
        }
    }
}
