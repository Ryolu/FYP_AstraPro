using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameover : MonoBehaviour {

    public float elapseTime; //setting the elapsed time for running
    public float endTime = 2.0f; //when reach end time it will go to another scene

	// Update is called once per frame
	void Update () {
        elapseTime += Time.deltaTime;
        if (elapseTime > endTime)
        {
            Menu_Manager.Instance.HighScore(); //after timer ends go to highscore
        }
		
	}
}
