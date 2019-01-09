using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameover : MonoBehaviour {

    public float elapseTime;
    public float endTime = 2.0f;
	// Update is called once per frame
	void Update () {
        elapseTime += Time.deltaTime;
        if (elapseTime > endTime)
        {
            Menu_Manager.Instance.HighScore();
        }
		
	}
}
