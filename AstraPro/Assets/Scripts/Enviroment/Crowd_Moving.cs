using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd_Moving : MonoBehaviour {
    public float movingSpeed = 1.0f;

    private void OnEnable()
    {
        movingSpeed = Random.Range(20.0f, 30.0f);
        Invoke("Destroy",15f);    
    }
    private void Destroy()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update ()
    {
        
       transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);
	}
}
