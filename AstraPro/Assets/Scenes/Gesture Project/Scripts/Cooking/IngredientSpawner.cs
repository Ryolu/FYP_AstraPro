using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour {

    public Ingredient ingredient;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(ingredient.ingredientSO.prefab, other.transform.GetChild(0).gameObject.transform);
    }
}
