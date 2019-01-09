using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Options : MonoBehaviour {
    public Slider BGM, SFX;

	// Use this for initialization
	void Start () {
        Audio_Manager.Instance.BGM = BGM;
        Audio_Manager.Instance.SFX = SFX;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
