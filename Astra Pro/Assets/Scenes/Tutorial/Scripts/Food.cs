using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class that takes in a FoodSO ScriptableObject
/// to use the data inside
/// </summary>

public class Food : MonoBehaviour {

    public FoodSO foodSO;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Attached to buttons to generate food
    /// </summary>
    /// <param name="foodSO"></param>
    public void GenerateFood(FoodSO foodSO)
    {
        GameObject food = ObjectPool.instance.GetPooledObject(foodSO);
    }
}
