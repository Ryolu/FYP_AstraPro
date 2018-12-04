using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirDetection : GestureDetection {

    int counter2 = 0;
	
	// Update is called once per frame
	void Update () {

        if (first.isHere && !second.isHere)
            counter++;
        else if (!first.isHere && second.isHere)
            counter2++;

        if (counter >= 5 && counter2 >= 5)
            Stir();

    }

    /// <summary>
    /// This function is called when the player
    /// does a certain number of stirring actions
    /// </summary>
    void Stir()
    {
        // Do stir stuff here

        Debug.Log("Stirring");
        counter2 = 0;
        DoGesture();
    }
}
