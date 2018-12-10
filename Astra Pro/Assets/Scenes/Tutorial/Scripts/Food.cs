using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : ScriptableObject {

    public string name = "Default";
    public Ingredient[] ingredientList;

    public bool[] cookingSteps;
}
