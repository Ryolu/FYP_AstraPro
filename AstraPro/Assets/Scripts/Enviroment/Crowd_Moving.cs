using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd_Moving : MonoBehaviour {
    //Moving Speed for the crowd
    public float movingSpeed = 1.0f;

    private void OnEnable()
    {
        //Change it to random range for human moving speed
        movingSpeed = Random.Range(2.0f, 5.0f);
        //After 5 second call functions
        Invoke("Destroy",5f);    
    }
    private void Destroy()
    {
        //Just set game object to false
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update ()
    {
        //if game is pause stop the game object from moving
        if (PauseManager.Instance != null && PauseManager.Instance.isPaused) return;
        //keep customer for moving around
        transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);
	}
}
