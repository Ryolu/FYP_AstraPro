using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopDetection : GestureDetection {

	// Update is called once per frame
	void Update () {

		if (first.isHere && !second.isHere)
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
        DoGesture();
    }
}
