using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopTrigger : MonoBehaviour {

    public bool isHere = false;

    void OnTriggerEnter(Collider other)
    {
        isHere = true;
    }
    private void LateUpdate()
    {
        isHere = false;
    }
}
