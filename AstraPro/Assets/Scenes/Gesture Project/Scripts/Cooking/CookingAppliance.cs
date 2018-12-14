using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingAppliance : MonoBehaviour {

    /// <summary>
    /// The transform to display the buttons which determine which food will be cooked
    /// </summary>
    public Transform foodButtonParent;
    /// <summary>
    /// The transform to display the ingredients left to put into this appliance
    /// </summary>
    public Transform ingredientDisplayParent;
    /// <summary>
    /// The image which displays which food is being cooked
    /// </summary>
    public Image foodDisplay;
    /// <summary>
    /// The list of foods which can be cooked by this appliance
    /// </summary>
    public List<Food> foodList;
    /// <summary>
    /// The prefab for displaying the buttons for the list of foods
    /// </summary>
    public GameObject foodButtonPrefab;
    /// <summary>
    /// The prefab for displaying the ingredients left
    /// </summary>
    public GameObject ingredientDisplayPrefab;
    
    bool isCooking;
    float timer;
    float cleanTimer;
    Food selectedFood;
    List<Ingredient> ingredients;

	// Use this for initialization
	void Start () {
        NewFood();
	}
	
	// Update is called once per frame
	void Update () {

        if (timer > 0)
            timer -= Time.deltaTime;
        else
            IsDone();

        if (cleanTimer > 0)
            cleanTimer -= Time.deltaTime;

	}

    /// <summary>
    /// This adds the ingredient to the cooking appliance in preparation for cooking
    /// </summary>
    /// <param name="ingredient"></param>
    void Added(Ingredient ingredient)
    {
        if (!isCooking && cleanTimer < 0)
        {
            // If the current selected food to cook has this ingredient and 
            // there isn't already the ingredient in the cooking appliance
            if (selectedFood.foodSO.ingredientList.Contains(ingredient.ingredientSO) && !ingredients.Contains(ingredient))
                ingredients.Add(ingredient);

            // Once there are all the needed ingredients
            if (ingredients.Count == selectedFood.foodSO.ingredientList.Count)
            {
                isCooking = true;
                timer = selectedFood.foodSO.timer;
                cleanTimer = selectedFood.foodSO.cleanTimer;
            }
        }
    }

    /// <summary>
    /// This is called when the player has finished cooking and the appliance is ready to be used again.
    /// </summary>
    void NewFood()
    {
        isCooking = false;
        timer = 0;
        cleanTimer = 0;
        ingredients = new List<Ingredient>();
        selectedFood = null;
    }

    /// <summary>
    /// This happens when the player did not make any mistakes and the cooking timer hits zero
    /// i.e. the food is done cooking
    /// </summary>
    void IsDone()
    {
        isCooking = false;

        // Do Food stuffs here

        NewFood();
    }

    /// <summary>
    /// This happens when the player adds in the wrong ingredient.
    /// It resets all the ingredients in this cooking appliance.
    /// </summary>
    void Failed()
    {
        // Do Failed stuffs here

        NewFood();
    }

    void ChooseFood(Food food)
    {
        selectedFood = food;

        // Do other stuff which happens when a food is selected

        NewFood();
    }
}
