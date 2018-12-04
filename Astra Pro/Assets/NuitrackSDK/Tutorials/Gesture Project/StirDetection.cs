using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirDetection : MonoBehaviour {

    [HideInInspector] public GestureTrigger vert;
    [HideInInspector] public GestureTrigger hori;

    int counter;

    // Use this for initialization
    void Start () {
        counter = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (vert.isHere && !hori.isHere)
            counter++;

        if (counter >= 10)
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
        counter = 0;
    }
}
