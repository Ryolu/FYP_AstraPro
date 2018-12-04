using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopDetection : MonoBehaviour {

    [HideInInspector] public ChopTrigger top;
    [HideInInspector] public ChopTrigger bottom;

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
            AmazeBalls();
    }

    void AmazeBalls()
    {
        Debug.Log("Chopping");
        counter = 0;
    }
}
