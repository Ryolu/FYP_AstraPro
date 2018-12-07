using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public Vector3 dir;

    // Disable itself after 2 seconds of being active
    private void OnEnable()
    {
        Invoke("Destroy", 2f);
    }

    private void Destroy()
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
        transform.position += dir;
    }
}
