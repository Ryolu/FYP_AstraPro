using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour {

    public GameObject cam;

	// Use this for initialization
	void Start () {
        transform.LookAt(cam.transform.position - 1000 * cam.transform.forward);
    }
}
