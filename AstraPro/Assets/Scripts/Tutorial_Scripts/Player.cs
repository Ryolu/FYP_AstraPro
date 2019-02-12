using UnityEngine;

/// <summary>
/// This class is used for collision between Player and Angry Customers' Projectiles in GameLevel and Tutorial Scenes.
/// This class is also used for easy access to Main Camera(Child) in GameLevel and Tutorial Scenes.
/// 
/// Can be found attached in Player.
/// </summary>
public class Player : MonoBehaviour
{
    public static Player Instance;

    private void Awake()
    {
        // Set instance for other Scripts to access
        Instance = this;
    }

    /// <summary>
    /// This deducts Score when Player gets hit by Angry Customers' Projectile.
    /// Shows current score through satisfying bar.
    /// And destroy the projectiles by calling Destroy() in Projectile Script.
    /// </summary>
    /// <param name="other"> The collider component in the Projectile. </param>
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