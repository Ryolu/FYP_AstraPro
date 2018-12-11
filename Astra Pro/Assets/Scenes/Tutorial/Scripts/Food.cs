using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

    public FoodSO foodSO;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateFood(FoodSO foodSO)
    {
        GameObject food = ObjectPool.instance.GetPooledObject(foodSO);
    }
}
