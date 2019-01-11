﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class Pointer1 : MonoBehaviour
{
    public enum Hands { left = 0, right = 1 };

    [SerializeField]
    Hands currentHand;

    [Header ("Visualization")]
    [SerializeField]
    RectTransform baseRect;

    [SerializeField]
    Image background;

    [SerializeField]
    Sprite defaultSprite;

    [SerializeField]
    Sprite pressSprite;

    bool active = false;
    bool press = false;

    [Header("Raycasting")]

    [SerializeField]
    Camera cam;

    Button selectedButton;

    PointerEventData eventData = new PointerEventData(null);
    List<RaycastResult> raycastResults = new List<RaycastResult>();

    [SerializeField]
    float dragSensitivity = 5f;

    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject foodListPanel;

    //[SerializeField] private Transform hand;
    //[SerializeField] private GameObject rightHandModel;
    //[SerializeField] private GameObject leftHandModel;
    private Transform hitTransform;
    private GameObject ingredient;
    private IngredientSO ingredientSO;
    private FoodSO foodSO;
    private GameObject cookingAppliance;
    private float elapsedTime;
    private float endTime = 0.75f;
    private Gradient greyGreenGradient;
    private Image timerImage;

    private void Start()
    {      
        NuitrackManager.onHandsTrackerUpdate += NuitrackManager_onHandsTrackerUpdate;
        dragSensitivity *= dragSensitivity;
        background.sprite = defaultSprite;
        InitiateColor();
        timerImage = transform.GetChild(0).GetComponent<Image>();
    }

    // Initiate Gradients, which is used to change color based on fillAmount of timerImage
    private void InitiateColor()
    {
        greyGreenGradient = new Gradient();
        var ck1 = new GradientColorKey[2];
        ck1[0].color = Color.grey;
        ck1[0].time = 0f;

        ck1[1].color = Color.green;
        ck1[1].time = 1f;

        var ak1 = new GradientAlphaKey[0];

        greyGreenGradient.SetKeys(ck1, ak1);
    }

    private void OnDestroy()
    {
        NuitrackManager.onHandsTrackerUpdate -= NuitrackManager_onHandsTrackerUpdate;
    }

    private void DropItem(GameObject item)
    {
        // Change Ingredient Sprite back to Hand Sprite
        background.sprite = defaultSprite;

        // Disable highlight on selected ingredient
        var o = item.GetComponentsInChildren<Outline>();
        foreach (var oL in o)
        {
            oL.selected = false;
            oL.color = 0;
        }

        if (item == ingredient)
        {
            // Reset Ingredient
            ingredient = null;
            ingredientSO = null;
        }
        else if (item == cookingAppliance)
        {
            // Reset Food
            cookingAppliance = null;
            foodSO = null;
        }
    }

    private void ShowOutline(GameObject parent)
    {
        // Enable highlight for hit object and its children
        var outlines = parent.GetComponentsInChildren<Outline>();
        foreach (var outline in outlines)
        {
            outline.enabled = true;
        }
    }

    private void NuitrackManager_onHandsTrackerUpdate(nuitrack.HandTrackerData handTrackerData)
    {
        active = false;
        press = false;

        if (handTrackerData != null)
        {
            nuitrack.UserHands userHands = handTrackerData.GetUserHandsByID(CurrentUserTracker.CurrentUser);

            if (userHands != null)
            {
                if (currentHand == Hands.right && userHands.RightHand != null)
                {
                    baseRect.anchoredPosition = new Vector2(userHands.RightHand.Value.X * Screen.width, -userHands.RightHand.Value.Y * Screen.height);
                    active = true;
                    press = userHands.RightHand.Value.Click;

                    #region Right Hand Model Code (Comment-ed)
                    //if(rightHandModel.activeSelf)
                    //{
                    //    // Place Model on Icon when active
                    //    rightHandModel.transform.position = transform.position;

                    //    // When player's right hand is too high, hide the model (For easier menu interaction)
                    //    if(userHands.RightHand.Value.Y <= 0.25f)
                    //    {
                    //        rightHandModel.SetActive(false);
                    //    }
                    //}
                    //else
                    //{
                    //    if(userHands.RightHand.Value.Y > 0.25f)
                    //    {
                    //        rightHandModel.SetActive(true);
                    //    }
                    //}
                    #endregion // Right Hand Model Code End
                }
                else if (currentHand == Hands.left && userHands.LeftHand != null)
                {                    
                    baseRect.anchoredPosition = new Vector2(userHands.LeftHand.Value.X * Screen.width, -userHands.LeftHand.Value.Y * Screen.height);
                    active = true;
                    press = userHands.LeftHand.Value.Click;

                    #region Left Hand Model Code (Comment-ed)
                    //if (leftHandModel.activeSelf)
                    //{
                    //    // Place Model on Icon when active
                    //    leftHandModel.transform.position = transform.position;

                    //    // When player's left hand is too high, hide the model (For easier menu interaction)
                    //    if (userHands.LeftHand.Value.Y <= 0.25f)
                    //    {
                    //        leftHandModel.SetActive(false);
                    //    }
                    //}
                    //else
                    //{
                    //    if (userHands.LeftHand.Value.Y > 0.25f)
                    //    {
                    //        leftHandModel.SetActive(true);
                    //    }
                    //}
                    #endregion // Left Hand Model Code End
                }
            }
        }

        // Show Image
        background.enabled = active;

        if(active)
        {
            if(!ingredientSO && !foodSO)
            {
                background.sprite = defaultSprite;
            }
        }
        else
        {
            return;
        }

        #region 3D raycast (Comment-ed)
        //// Only Raycast to objects when food list panel is not active
        //if (!foodListPanel.activeSelf)
        //{
        //    RaycastHit hit;
        //    var landingRay = new Ray(transform.position, (transform.position - cam.transform.position).normalized);

        //    //// Draw ray in Scene view for Debug
        //    Debug.DrawRay(transform.position, (transform.position - cam.transform.position).normalized * 10f);

        //    // Raycast 10 units with landingRay
        //    if (Physics.Raycast(landingRay, out hit, 10f))
        //    {
        //        //// Check hit which object
        //        //Debug.Log(hit.transform.name);

        //        #region Highlight Code Segment (Comment-ed)
        //        //// If previously got hit other object
        //        //if (hitTransform)
        //        //{
        //        //    // Disable highlight for hit object and its children
        //        //    var o = hitTransform.GetComponentsInChildren<Outline>();
        //        //    foreach (var oL in o)
        //        //    {
        //        //        if (!oL.selected)
        //        //        {
        //        //            oL.enabled = false;
        //        //        }
        //        //    }
        //        //}
        //        //hitTransform = hit.transform;

        //        //if (!ingredientSO && !foodSO)
        //        //{
        //        //    // If selecting Cooking Appliance(Frying Pan, Pot 1, Pot 2)
        //        //    if (LevelManager.Instance.cookingAppliances.Select(x => x.gameObject.GetInstanceID()).Contains(hit.transform.gameObject.GetInstanceID()))
        //        //    {
        //        //        // Enable highlight for hit object and its children
        //        //        var outlines = hitTransform.GetComponentsInChildren<Outline>();
        //        //        foreach (var outline in outlines)
        //        //        {
        //        //            outline.enabled = true;
        //        //        }
        //        //    }
        //        //    else
        //        //    {
        //        //        // Check if any Cooking Appliance is waiting for ingredient input
        //        //        var chosenFood = false;
        //        //        if (LevelManager.Instance.cookingAppliances.Select(x => x.selectedFood).Any(y => y != null))
        //        //        {
        //        //            chosenFood = true;
        //        //        }

        //        //        // There is Cooking Appliance waiting for ingredient input
        //        //        if (chosenFood)
        //        //        {
        //        //            // If selecting Ingredients(Banana Leaves, Tofus, Eggs, Noodle, Spice, Fishes, Prawns)
        //        //            if (LevelManager.Instance.ingredients.Select(x => x.gameObject.GetInstanceID()).Contains(hit.transform.gameObject.GetInstanceID()))
        //        //            {
        //        //                // Enable highlight for hit object and its children
        //        //                var outlines = hitTransform.GetComponentsInChildren<Outline>();
        //        //                foreach (var outline in outlines)
        //        //                {
        //        //                    outline.enabled = true;
        //        //                }
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //        //else if (ingredientSO)
        //        //{
        //        //    // If selecting Cooking Appliance(Frying Pan, Pot 1, Pot 2)
        //        //    if (LevelManager.Instance.cookingAppliances.Select(x => x.gameObject.GetInstanceID()).Contains(hit.transform.gameObject.GetInstanceID()))
        //        //    {
        //        //        // Enable highlight for hit object and its children
        //        //        var outlines = hitTransform.GetComponentsInChildren<Outline>();
        //        //        foreach (var outline in outlines)
        //        //        {
        //        //            outline.enabled = true;
        //        //        }
        //        //    }
        //        //}
        //        //else if (foodSO)
        //        //{
        //        //    // If selecting Customer
        //        //    if (CustomerSpawner.Instance.customerDic.Select(x => x.Value.gameObject.GetInstanceID()).Contains(hit.transform.parent.gameObject.GetInstanceID()))
        //        //    {
        //        //        // Enable highlight for hit object and its children
        //        //        var outlines = hitTransform.GetComponentsInChildren<Outline>();
        //        //        foreach (var outline in outlines)
        //        //        {
        //        //            outline.enabled = true;
        //        //        }
        //        //    }
        //        //}
        //        #endregion // Highlight Code Segment End

        //        #region Cooking Control Code Segment

        //        #region Toggle Style Control (Comment-ed)
        //        //if (press)
        //        //{
        //        //    elapsedTime += Time.deltaTime;
        //        //    if (elapsedTime >= endTime)
        //        //    {
        //        //        // If selecting Cooking Appliance(Frying Pan, Pot 1, Pot 2)
        //        //        if (LevelManager.Instance.cookingAppliances.Select(x => x.gameObject.GetInstanceID()).Contains(hit.transform.gameObject.GetInstanceID()))
        //        //        {
        //        //            var app = hit.transform.GetComponent<CookingAppliance>();
        //        //            if (!app.isDone)
        //        //            {
        //        //                if (!ingredientSO)
        //        //                {
        //        //                    // Open up Food list to choose "food to cook"
        //        //                    app.OpenCloseFoodMenu(true);
        //        //                }
        //        //                else
        //        //                {
        //        //                    // Add food(ingredient) into the appliance stated above
        //        //                    app.AddIngredient(ingredientSO);
        //        //                    // Disable highlight on selected ingredient
        //        //                    var o = ingredient.GetComponentsInChildren<Outline>();
        //        //                    foreach (var oL in o)
        //        //                    {
        //        //                        oL.selected = false;
        //        //                        oL.color = 0;
        //        //                    }
        //        //                    ingredient = null;
        //        //                    ingredientSO = null;
        //        //                }
        //        //            }
        //        //            else
        //        //            {
        //        //                // If previously selected another cooking Appliance
        //        //                if (cookingAppliance)
        //        //                {
        //        //                    // Disable highlight on selected ingredient
        //        //                    var O = cookingAppliance.GetComponentsInChildren<Outline>();
        //        //                    foreach (var oL in O)
        //        //                    {
        //        //                        oL.selected = false;
        //        //                        oL.color = 0;
        //        //                    }
        //        //                }
        //        //                // Select food and store it for serving customer later
        //        //                cookingAppliance = hit.transform.gameObject;
        //        //                foodSO = app.TakeFood();
        //        //                // Enable highlight on selected cooking Appliance
        //        //                var o = cookingAppliance.GetComponentsInChildren<Outline>();
        //        //                foreach (var oL in o)
        //        //                {
        //        //                    oL.selected = true;
        //        //                    oL.color = 1;
        //        //                }
        //        //            }
        //        //        }
        //        //        // If selecting ingredients(Banana Leaves, Tofus, Eggs, Noodle, Spice, Fishes, Prawns)
        //        //        else if (LevelManager.Instance.ingredients.Select(x => x.gameObject.GetInstanceID()).Contains(hit.transform.gameObject.GetInstanceID()))
        //        //        {
        //        //            bool chosenFood = false;
        //        //            if (LevelManager.Instance.cookingAppliances.Select(x => x.selectedFood).Distinct().Any())
        //        //                chosenFood = true;
        //        //            if (!chosenFood)
        //        //                return;
        //        //            // If previously selected another ingredient
        //        //            if (ingredient)
        //        //            {
        //        //                // Disable highlight on selected ingredient
        //        //                var O = ingredient.GetComponentsInChildren<Outline>();
        //        //                foreach (var oL in O)
        //        //                {
        //        //                    oL.selected = false;
        //        //                    oL.color = 0;
        //        //                }
        //        //                ingredient = null;
        //        //                ingredientSO = null;
        //        //            }
        //        //            // Set ingredient
        //        //            ingredient = hit.transform.gameObject;
        //        //            ingredientSO = ingredient.GetComponent<Ingredient>().ingredientSO;
        //        //            background.sprite = ingredientSO.sprite;
        //        //            // Enable highlight on selected ingredient
        //        //            var o = ingredient.GetComponentsInChildren<Outline>();
        //        //            foreach (var oL in o)
        //        //            {
        //        //                oL.selected = true;
        //        //                oL.color = 1;
        //        //            }
        //        //        }
        //        //        // If selecting customer
        //        //        else if (CustomerSpawner.Instance.customerDic.Select(x => x.Value.gameObject.GetInstanceID()).Contains(hit.transform.parent.gameObject.GetInstanceID()))
        //        //        {
        //        //            var customer = hit.transform.gameObject.GetComponentInParent<Customer>();
        //        //            if (foodSO)
        //        //            {
        //        //                if (foodSO == customer.foodOrdered)
        //        //                {
        //        //                    // Served correct food, Add Score
        //        //                    Score.instance.Profit(customer.foodOrdered);
        //        //                    // Reset cooking Appliance status
        //        //                    cookingAppliance.GetComponent<CookingAppliance>().NewFood();
        //        //                }
        //        //                else
        //        //                {
        //        //                    // Served wrong food, Decrease Rate
        //        //                    Score.instance.Rate -= 0.1f;
        //        //                }
        //        //                // Customer leaves
        //        //                customer.Leave(customer.customerId);
        //        //                // Disable highlight on selected cooking Appliance
        //        //                var o = cookingAppliance.GetComponentsInChildren<Outline>();
        //        //                foreach (var oL in o)
        //        //                {
        //        //                    oL.selected = false;
        //        //                    oL.color = 0;
        //        //                }
        //        //                cookingAppliance = null;
        //        //                foodSO = null;
        //        //            }
        //        //        }
        //        //        elapsedTime = 0f;
        //        //    }
        //        //}
        //        #endregion // Toggle Style Control End

        //        #region Grab & Drag Style Control (Comment-ed)
        //        //// Grip
        //        //if (press)
        //        //{
        //        //    elapsedTime += Time.deltaTime;

        //        //    if (elapsedTime >= endTime)
        //        //    {
        //        //        elapsedTime = 0f;

        //        //        // When not carrying anything
        //        //        if (!ingredientSO && !foodSO)
        //        //        {
        //        //            // If selecting Cooking Appliance(Frying Pan, Pot 1, Pot 2)
        //        //            if (LevelManager.Instance.cookingAppliances.Select(x => x.gameObject.GetInstanceID()).Contains(hit.transform.gameObject.GetInstanceID()))
        //        //            {
        //        //                var app = hit.transform.GetComponent<CookingAppliance>();

        //        //                // Haven't done cooking food
        //        //                if (!app.isDone)
        //        //                {
        //        //                    // Open up Food list to choose "food to cook"
        //        //                    app.OpenCloseFoodMenu(true);
        //        //                }
        //        //                // Done cooking food
        //        //                else
        //        //                {
        //        //                    // Select food and store it for serving customer
        //        //                    cookingAppliance = hit.transform.gameObject;
        //        //                    foodSO = app.TakeFood();

        //        //                    // Change Hand Sprite to Food Sprite
        //        //                    background.sprite = foodSO.sprite;

        //        //                    // Enable highlight on selected cooking Appliance
        //        //                    var o = cookingAppliance.GetComponentsInChildren<Outline>();
        //        //                    foreach (var oL in o)
        //        //                    {
        //        //                        oL.selected = true;
        //        //                        oL.color = 1;
        //        //                    }
        //        //                }
        //        //            }
        //        //            // If selecting ingredients(Banana Leaves, Tofus, Eggs, Noodle, Spice, Fishes, Prawns)
        //        //            else if (LevelManager.Instance.ingredients.Select(x => x.gameObject.GetInstanceID()).Contains(hit.transform.gameObject.GetInstanceID()))
        //        //            {
        //        //                // Check if any Cooking Appliance is waiting for ingredient input
        //        //                var chosenFood = false;
        //        //                if (LevelManager.Instance.cookingAppliances.Select(x => x.selectedFood).Distinct().Any())
        //        //                {
        //        //                    chosenFood = true;
        //        //                }

        //        //                // There is Cooking Appliance waiting for ingredient input
        //        //                if (chosenFood)
        //        //                {
        //        //                    // Set ingredient
        //        //                    ingredient = hit.transform.gameObject;
        //        //                    ingredientSO = ingredient.GetComponent<Ingredient>().ingredientSO;

        //        //                    // Change Hand Sprite to Ingredient Sprite
        //        //                    background.sprite = ingredientSO.sprite;

        //        //                    // Enable highlight on selected ingredient
        //        //                    var o = ingredient.GetComponentsInChildren<Outline>();
        //        //                    foreach (var oL in o)
        //        //                    {
        //        //                        oL.selected = true;
        //        //                        oL.color = 1;
        //        //                    }
        //        //                }
        //        //                // No Cooking Appliance is waiting for ingredient input
        //        //                else
        //        //                {
        //        //                    // Do nothing
        //        //                    return;
        //        //                }
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //        //// Released Grip
        //        //else
        //        //{
        //        //    elapsedTime += Time.deltaTime;

        //        //    // When Carrying ingredient
        //        //    if (ingredientSO)
        //        //    {
        //        //        // If selecting Cooking Appliance(Frying Pan, Pot 1, Pot 2)
        //        //        if (LevelManager.Instance.cookingAppliances.Select(x => x.gameObject.GetInstanceID()).Contains(hit.transform.gameObject.GetInstanceID()))
        //        //        {
        //        //            var app = hit.transform.GetComponent<CookingAppliance>();

        //        //            if (!app.isDone)
        //        //            {
        //        //                // Add ingredient into the Cooking Appliance
        //        //                app.AddIngredient(ingredientSO);

        //        //                DropItem(ingredient);
        //        //            }
        //        //        }
        //        //        // Released Grip too long and Ray cast still haven't hit any Cooking Appliance
        //        //        else if (elapsedTime >= endTime)
        //        //        {
        //        //            DropItem(ingredient);
        //        //        }
        //        //    }
        //        //    // When Carrying food
        //        //    else if (foodSO)
        //        //    {
        //        //        // If selecting customer
        //        //        if (CustomerSpawner.Instance.customerDic.Select(x => x.Value.gameObject.GetInstanceID()).Contains(hit.transform.parent.gameObject.GetInstanceID()))
        //        //        {
        //        //            var customer = hit.transform.gameObject.GetComponentInParent<Customer>();

        //        //            if (foodSO == customer.foodOrdered)
        //        //            {
        //        //                // Served correct food, Add Score
        //        //                Score.Instance.Profit(customer.foodOrdered);
        //        //            }
        //        //            else
        //        //            {
        //        //                // Served wrong food, Decrease Rate
        //        //                Score.Instance.rate -= 0.1f;
        //        //            }

        //        //            // Reset cooking Appliance status
        //        //            cookingAppliance.GetComponent<CookingAppliance>().NewFood();

        //        //            // Customer leaves
        //        //            customer.Leave(customer.customerId);

        //        //            DropItem(cookingAppliance);
        //        //        }
        //        //        // Released Grip too long and Ray cast still haven't hit any Cooking Appliance
        //        //        else if (elapsedTime >= endTime)
        //        //        {
        //        //            DropItem(cookingAppliance);
        //        //        }
        //        //    }
        //        //}
        //        #endregion // Grab & Drag Style Control End

        //        #endregion // Cooking Control Code Segment End
        //    }
        //    else
        //    {
        //        // Disable highlight for every objects that have outline
        //        var o = FindObjectsOfType<Outline>();
        //        foreach (var oL in o)
        //        {
        //            if (!oL.selected)
        //            {
        //                oL.enabled = false;
        //            }
        //        }
        //    }
        //}
        #endregion // 3D raycast End

        var pointOnScreenPosition = (Vector2)cam.WorldToScreenPoint(transform.position);
        eventData.delta = pointOnScreenPosition - eventData.position;
        eventData.position = pointOnScreenPosition;

        raycastResults.Clear();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        Button newButton = null;

        for (int i = 0; i < raycastResults.Count && newButton == null; i++)
            newButton = raycastResults[i].gameObject.GetComponent<Button>();

        // Calls event override functions in imageitem1.cs
        if (newButton != selectedButton)
        {
            if (selectedButton != null)
            {
                selectedButton.OnPointerExit(eventData);
                elapsedTime = 0f;
                timerImage.fillAmount = 0f;
            }
        
            selectedButton = newButton;
        
            if (selectedButton != null)
                selectedButton.OnPointerEnter(eventData);
        }
        else if (selectedButton != null)
        {
            elapsedTime += Time.deltaTime;

            // Reduce fillAmount of Timer Filler Image(visual feedback) over waitTiming
            timerImage.fillAmount += (1f / endTime) * Time.deltaTime;

            // Left more than half the time -> Image turning from green to yellow
            //if (timerImage.fillAmount >= 0.5f)
            {
                timerImage.color = greyGreenGradient.Evaluate(timerImage.fillAmount);
            }

            #region Highlight Code
            // If previously got hit other object
            if (hitTransform)
            {
                // Disable highlight for hit object and its children
                var o = hitTransform.GetComponentsInChildren<Outline>();
                foreach (var oL in o)
                {
                    if (!oL.selected)
                    {
                        oL.enabled = false;
                    }
                }
            }

            if (!foodListPanel.activeSelf)
            {
                // If selecting Cooking Appliance(Frying Pan, Pot 1, Pot 2)
                if (LevelManager.Instance.cookingAppliances.Any(x => x.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()))
                {
                    var something = LevelManager.Instance.cookingAppliances.Where(x => x.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()).ToList();

                    if (something.Count != 1)
                        return;

                    var app = something[0].GetComponent<CookingAppliance>();

                    hitTransform = app.transform;
                    ShowOutline(app.gameObject);
                }
                // If selecting ingredients(Banana Leaves, Tofus, Eggs, Noodle, Spice, Fishes, Prawns)
                else if (LevelManager.Instance.ingredients.Any(x => x.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()))
                {
                    var something = LevelManager.Instance.ingredients.Where(x => x.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()).ToList();

                    if (something.Count != 1)
                        return;

                    var ingre = something[0].GetComponent<Ingredient>();

                    hitTransform = ingre.transform;
                    ShowOutline(ingre.gameObject);
                }
                // When Carrying food
                else if (foodSO)
                {
                    // If selecting customer
                    if (CustomerSpawner.Instance.customerDic.Any(x => x.Value.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()))
                    {
                        var something = CustomerSpawner.Instance.customerDic.Where(x => x.Value.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()).ToList();

                        if (something.Count != 1)
                            return;

                        var customer = something[0].Value.GetComponent<Customer>();

                        hitTransform = customer.transform;
                        ShowOutline(customer.gameObject);
                    }
                }
            }
            #endregion Highlight Code

            if (elapsedTime >= endTime)
            {
                elapsedTime = 0f;
                timerImage.fillAmount = 0f;

                if (!foodListPanel.activeSelf)
                {
                    // If selecting Cooking Appliance(Frying Pan, Pot 1, Pot 2)
                    if (LevelManager.Instance.cookingAppliances.Any(x => x.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()))
                    {
                        var something = LevelManager.Instance.cookingAppliances.Where(x => x.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()).ToList();

                        if (something.Count != 1)
                            return;

                        var app = something[0].GetComponent<CookingAppliance>();

                        // When not carrying ingredient
                        if (!ingredientSO)
                        {
                            // Haven't done cooking food
                            if (!app.isDone)
                            {
                                // Open up Food list to choose "food to cook"
                                app.OpenCloseFoodMenu(true);
                            }
                            // Done cooking food
                            else
                            {
                                // If previously selected another cooking Appliance
                                if (cookingAppliance)
                                {
                                    DropItem(cookingAppliance);
                                }

                                // Select food and store it for serving customer
                                cookingAppliance = app.gameObject;
                                foodSO = app.TakeFood();

                                // Change Hand Sprite to Food Sprite
                                background.sprite = foodSO.sprite;

                                // Enable highlight on selected cooking Appliance
                                var o = cookingAppliance.GetComponentsInChildren<Outline>();
                                foreach (var oL in o)
                                {
                                    oL.selected = true;
                                    oL.color = 1;
                                }
                            }
                        }
                        // When carrying ingredient
                        else
                        {
                            if (!app.isDone)
                            {
                                // Add ingredient into the Cooking Appliance
                                app.AddIngredient(ingredientSO);

                                DropItem(ingredient);
                            }
                        }
                    }
                    // If selecting ingredients(Banana Leaves, Tofus, Eggs, Noodle, Spice, Fishes, Prawns)
                    else if (LevelManager.Instance.ingredients.Any(x => x.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()))
                    {
                        var something = LevelManager.Instance.ingredients.Where(x => x.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()).ToList();

                        if (something.Count != 1)
                            return;

                        var ingre = something[0].GetComponent<Ingredient>();

                        // Check if any Cooking Appliance is waiting for ingredient input
                        if (LevelManager.Instance.cookingAppliances.Any(x => x.selectedFood != null))
                        {
                            // There is Cooking Appliance waiting for ingredient input

                            // If previously selected another ingredient
                            if (ingredient)
                            {
                                DropItem(ingredient);
                            }

                            // Set ingredient
                            ingredient = ingre.gameObject;
                            ingredientSO = ingre.ingredientSO;

                            // Change Hand Sprite to Ingredient Sprite
                            background.sprite = ingredientSO.sprite;

                            // Enable highlight on selected ingredient
                            var o = ingredient.GetComponentsInChildren<Outline>();
                            foreach (var oL in o)
                            {
                                oL.selected = true;
                                oL.color = 1;
                            }
                        }
                    }
                    // When Carrying food
                    else if (foodSO)
                    {
                        // If selecting customer
                        if (CustomerSpawner.Instance.customerDic.Any(x => x.Value.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()))
                        {
                            var something = CustomerSpawner.Instance.customerDic.Where(x => x.Value.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()).ToList();

                            if (something.Count != 1)
                                return;

                            var customer = something[0].Value.GetComponent<Customer>();

                            if (!customer.fighting)
                            {
                                if (foodSO == customer.foodOrdered)
                                {
                                    // Served correct food, Add Score
                                    Score.Instance.Profit(customer.foodOrdered, customer.timerImage.fillAmount);

                                    // Customer leaves
                                    customer.Leave(customer.customerId);
                                }
                                else
                                {
                                    // Served wrong food, Decrease Rate
                                    Score.Instance.rate -= 0.1f;
                                    customer.fighting = true;

                                    customer.player = cam.transform.parent;
                                }

                                // Reset cooking Appliance status
                                cookingAppliance.GetComponent<CookingAppliance>().NewFood();

                                DropItem(cookingAppliance);
                            }
                        }
                    }
                }
                // Call Button OnClick()
                selectedButton.OnPointerClick(eventData);
            }

            #region Press 2D Button (Comment-ed)
            //if (press)
            //{
            //    elapsedTime += Time.deltaTime;

            //    if (elapsedTime >= endTime)
            //    {
            //        //if (eventData.delta.sqrMagnitude < dragSensitivity && !eventData.dragging)
            //        //{
            //        //eventData.dragging = true;
            //        selectedButton.OnPointerDown(eventData);
            //        selectedButton.OnPointerClick(eventData);
            //        //}

            //        //// Shoot bullet towards hand icon
            //        //GameObject Projectile = ObjectPool.instance.GetPooledObject(ProjectilePrefab);

            //        //if (!Projectile) return;

            //        //Projectile.transform.position = hand.position;
            //        //Projectile.transform.rotation = Quaternion.identity;
            //        //Projectile.GetComponent<Projectile>().dir = (background.transform.position - hand.position).normalized;

            //        elapsedTime = 0f;
            //    }
            //}
            ////else if (eventData.dragging)
            ////{
            //    //eventData.dragging = false;
            //    selectedButton.OnPointerUp(eventData);
            ////}

            ////selectedButton.OnDrag(eventData);
            #endregion // Press 2D Button End
        }
        else
        {
            // Disable highlight for every objects that have outline
            var ol = FindObjectsOfType<Outline>();
            foreach (var oL in ol)
            {
                if (!oL.selected)
                {
                    oL.enabled = false;
                }
            }
        }

        if (CustomerSpawner.Instance.customerDic.Any(x => x.Value.fighting == true))
        {
            var something = CustomerSpawner.Instance.customerDic.Where(x => x.Value.fighting == true).ToList();
            
            foreach(var pair in something)
            {
                var customer = pair.Value.GetComponent<Customer>();

                // Set player transform for customer to have a target to shoot at
                if (!customer.player)
                {
                    customer.player = cam.transform.parent;
                }
            }            

            if (!foodListPanel.activeSelf && press)
            {
                // Shoot bullet towards hand icon
                GameObject Projectile = ObjectPool.Instance.GetPooledObject(ProjectilePrefab);

                if (!Projectile) return;

                Projectile.transform.position = transform.position;
                Projectile.transform.rotation = Quaternion.identity;
                Projectile.GetComponent<Projectile>().dir = (transform.position - cam.transform.position).normalized;
            }
        }
    }
}
