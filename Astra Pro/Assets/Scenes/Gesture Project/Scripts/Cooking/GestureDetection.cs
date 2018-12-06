using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureDetection : MonoBehaviour {

    [HideInInspector]
    public GestureTrigger first;
    [HideInInspector]
    public GestureTrigger second;

    protected int counter;

	// Use this for initialization
	protected void Start () {
        counter = 0;
	}

    /// <summary>
    /// Resets any variable which is consistent throughout children
    /// </summary>
    protected void DoGesture()
    {
        counter = 0;
    }
}
