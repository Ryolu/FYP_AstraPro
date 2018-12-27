using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {
    public static Score instance;
    public float Overall = 100.0f;
    public float Otah = 5.0f;
    public float CurryFishHead = 10.0f;
    public float Laksa = 6.0f;
    public float MeeSiam = 5.5f; 
    public float Rate = 1.0f;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        Overall = 100.0f;
        Rate = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
