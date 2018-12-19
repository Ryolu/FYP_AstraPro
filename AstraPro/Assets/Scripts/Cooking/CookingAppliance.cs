﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookingAppliance : MonoBehaviour {

    /// <summary>
    /// The list of foods which can be cooked by this appliance
    /// </summary>
    public List<FoodSO> foodList;

    /// <summary>
    /// The transform to display the buttons which determine which food will be cooked
    /// This panel can be found in the main canvas
    /// </summary>
    [Tooltip("This panel can be found in the main canvas")]
    [SerializeField] Transform foodButtonPanel;
    /// <summary>
    /// The transform to display the ingredients left to put into this appliance
    /// This panel can be found in the canvas attached to this appliance
    /// </summary>
    [Tooltip("This panel can be found in the canvas attached to this appliance")]
    [SerializeField] Transform ingredientDisplayPanel;
    /// <summary>
    /// The image which displays which food is being cooked
    /// </summary>
    [SerializeField] Image foodDisplay;
    /// <summary>
    /// Panel containing the list of foods to choose from
    /// </summary>
    [Tooltip("Look for 'Food List Panel' in the main canvas")]
    [SerializeField] GameObject foodListPanel;
    /// <summary>
    /// The prefab for displaying the buttons for the list of foods
    /// </summary>
    [SerializeField] GameObject foodButtonPrefab;
    /// <summary>
    /// The prefab for displaying the ingredients left
    /// </summary>
    [SerializeField] GameObject ingredientDisplayPrefab;
    
    bool isCooking;
    float timer;
    float cleanTimer;
    FoodSO selectedFood;
    List<IngredientSO> ingredients;
    List<Image> ingredientDisplayList;

	// Use this for initialization
	void Start () {
        NewFood();
	}
	
	// Update is called once per frame
	void Update () {

        //if (timer > 0)
        //    timer -= Time.deltaTime;
        //else
        //    IsDone();
        //
        //if (cleanTimer > 0)
        //    cleanTimer -= Time.deltaTime;
        //else
        //    NewFood();

        if (Input.GetKeyUp(KeyCode.Space))
            OpenList();

        if (Input.GetKeyUp(KeyCode.R))
            NewFood();
    }

    /// <summary>
    /// This adds the ingredient to the cooking appliance in preparation for cooking
    /// </summary>
    /// <param name="ingredient"></param>
    public void Added(IngredientSO ingredientSO)
    {
        if (!isCooking && cleanTimer <= 0)
        {
            // If the current selected food to cook has this ingredient and 
            // there isn't already the ingredient in the cooking appliance
            if (selectedFood.ingredientList.Contains(ingredientSO) && !ingredients.Contains(ingredientSO))
            {
                ingredients.Add(ingredientSO);

                foreach (Image display in ingredientDisplayList)
                {
                    if (display.sprite == ingredientSO.sprite)
                    {
                        // Blacks out the ingredient the player is putting into the cooking appliance
                        Color color = display.color;
                        color = new Color(0, 0, 0);
                        display.color = color;
                    }
                }
            }
            else
                Failed();

            // Once there are all the needed ingredients
            if (ingredients.Count == selectedFood.ingredientList.Count)
            {
                isCooking = true;
                timer = selectedFood.timer;
                cleanTimer = selectedFood.cleanTimer;
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
        ingredients = new List<IngredientSO>();
        ingredientDisplayList = new List<Image>();
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

    public void ChooseFood(FoodSO foodSO)
    {
        foodListPanel.SetActive(false);

        if (selectedFood)
            return;
        else
            selectedFood = foodSO;

        // Do other stuff which happens when a food is selected
        foreach (IngredientSO ingredient in foodSO.ingredientList)
        {
            GameObject prefab = Instantiate(ingredientDisplayPrefab, ingredientDisplayPanel);
            Image ingredientImage = prefab.GetComponent<Image>();
            ingredientImage.sprite = ingredient.sprite;
            ingredientDisplayList.Add(ingredientImage);
        }

        ingredientDisplayPanel.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(2.9f, 1.2f * foodSO.ingredientList.Count / 2 + 0.5f); 
    }

    public void OpenList()
    {
        if (foodListPanel.activeInHierarchy || selectedFood)
            return;

        foodListPanel.SetActive(true);

        foreach (FoodSO foodSO in foodList)
        {
            GameObject prefab = Instantiate(foodButtonPrefab, foodButtonPanel);
            TextMeshProUGUI foodName = prefab.GetComponentInChildren<TextMeshProUGUI>();
            foodName.text = foodSO.foodName;
            Image foodImage = prefab.GetComponentInChildren<Image>();
            foodImage.sprite = foodSO.sprite;
            Button foodButton = prefab.GetComponentInChildren<Button>();
            foodButton.onClick.AddListener(() => ChooseFood(foodSO));
        }
    }
}
