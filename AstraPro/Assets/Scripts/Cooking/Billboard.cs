using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    public GameObject cam;

	// Use this for initialization
	void Start () {
        transform.LookAt(cam.transform);
	}
}
