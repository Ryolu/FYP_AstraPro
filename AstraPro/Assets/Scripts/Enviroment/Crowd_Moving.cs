using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd_Moving : MonoBehaviour {
    public float movingSpeed = 1.0f;

    private void OnEnable()
    {
        Invoke("Destroy",10f);    
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
