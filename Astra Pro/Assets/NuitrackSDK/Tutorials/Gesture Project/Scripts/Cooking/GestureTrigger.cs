using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureTrigger : MonoBehaviour {

    public enum GestureType
    {
        none,
        chop,
        stir
    }

    [HideInInspector] public bool isHere = false;
    public GestureType type;

    void OnTriggerEnter(Collider other)
    {
        isHere = true;
    }
    void LateUpdate()
    {
        isHere = false;
    }
}
