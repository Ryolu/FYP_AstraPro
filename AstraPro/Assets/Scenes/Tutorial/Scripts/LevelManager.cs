using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public List<CookingAppliance> cookingAppliances;
    public List<Ingredient> ingredients;

	void Awake ()
    {
        Instance = this;
	}
	
	void Update ()
    {
		
	}
}
