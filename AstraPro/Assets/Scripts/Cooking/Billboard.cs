using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    public GameObject cam;

	// Use this for initialization
	void Start () {
        //transform.LookAt(new Vector3(cam.transform.position.x, transform.position.y, cam.transform.position.z));
        transform.LookAt(cam.transform.position - 1000 * cam.transform.forward);
    }
}
