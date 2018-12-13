using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGestureManager : MonoBehaviour {

    public ChopDetection chop;
    public StirDetection stir;

	// Use this for initialization
	void Start () {
        //DisableAll();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Called in Start() so that no detection is switched on
    /// </summary>
    private void DisableAll()
    {
        chop.enabled = false;
        stir.enabled = false;
    }

    /// <summary>
    /// Called to switch on chopping gesture detection
    /// </summary>
    void OnChop()
    {
        chop.enabled = true;
    }

    /// <summary>
    /// Called to switch on stirring gesture detection
    /// </summary>
    void OnStir()
    {
        stir.enabled = true;
    }
}
