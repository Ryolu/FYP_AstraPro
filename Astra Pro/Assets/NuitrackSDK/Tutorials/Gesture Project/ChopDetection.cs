using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopDetection : MonoBehaviour {

    [HideInInspector] public GestureTrigger top;
    [HideInInspector] public GestureTrigger bottom;

    int counter;

    // Use this for initialization
    void Start () {
        
        counter = 0;

    }
	
	// Update is called once per frame
	void Update () {

		if (top.isHere && !bottom.isHere)
            counter++;
        
        if (counter >= 10)
            Chop();
    }

    /// <summary>
    /// This function is called when the player
    /// does a certain number of chopping actions
    /// </summary>
    void Chop()
    {
        // Do chop stuff here

        Debug.Log("Chopping");
        counter = 0;
    }
}
