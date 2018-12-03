using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopDetection : MonoBehaviour {

    public ChopTrigger top;
    public ChopTrigger bottom;

    int counter;

    // Use this for initialization
    void Start () {
        
        counter = 0;

    }
	
	// Update is called once per frame
	void Update () {

		if (top.isHere && !bottom.isHere)// && oldtop != oldbottom && oldtop != top.isHere)
            counter++;
        
        if (counter >= 10)
            AmazeBalls();

        //if (top.isHere)
        //    Debug.Log("T R I G G E R E D");
    }

    void AmazeBalls()
    {
        Debug.Log("Chopping");
        counter = 0;
    }
}
