using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaynNight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.isPaused) return;

        transform.RotateAround(Vector3.zero, Vector3.right, 10f * Time.deltaTime);
        transform.LookAt(Vector3.zero);
    }
}
