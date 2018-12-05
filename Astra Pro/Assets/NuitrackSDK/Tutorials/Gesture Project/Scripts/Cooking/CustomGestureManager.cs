using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGestureManager : MonoBehaviour {

    public ChopDetection chop;
    public StirDetection stir;

	// Use this for initialization
	void Start () {
        DisableAll();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void DisableAll()
    {
        chop.enabled = false;
        stir.enabled = false;
    }

    void OnChop()
    {
        chop.enabled = true;
    }

    void OnStir()
    {
        stir.enabled = true;
    }
}
