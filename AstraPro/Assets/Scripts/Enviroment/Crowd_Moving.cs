using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd_Moving : MonoBehaviour {
    public float movingSpeed = 1.0f;

    private void OnEnable()
    {
        movingSpeed = Random.Range(2.0f, 5.0f);
        Invoke("Destroy",5f);    
    }
    private void Destroy()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update ()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.isPaused) return;

        transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);
	}
}
