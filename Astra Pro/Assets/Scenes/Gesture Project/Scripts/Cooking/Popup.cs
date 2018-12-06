using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour {

    GameObject textChild;
    float countdown;

	// Use this for initialization
	void Start () {
        textChild = gameObject.transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (countdown > 0)
            countdown -= Time.deltaTime;
        else
            gameObject.SetActive(false);
	}

    private void OnEnable()
    {
        countdown = 3;
    }
}
