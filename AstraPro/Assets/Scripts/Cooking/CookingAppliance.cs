using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;



public class CookingAppliance : MonoBehaviour {

    /// <summary>
    /// The list of foods which can be cooked by this appliance
    /// </summary>
    public List<FoodSO> foodList;
    /// <summary>
    /// Currently selected food
    /// </summary>
    public FoodSO selectedFood;

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
    [Tooltip("This panel can be found in the main canvas")]
    [SerializeField] Transform ingredientPanel;
    /// <summary>
    /// The image which displays which food is being cooked
    /// </summary>
    [Tooltip("This image can be found in the canvas attached to this appliance under the Display Timer panel")]
    [SerializeField] Image foodDisplay;

    [Tooltip("This image can be found in the canvas attached to this appliance under the Display Timer panel")]
    [SerializeField] Image foodTimerFront;

    [Tooltip("This text can be found in the canvas attached to this appliance under the Display Timer panel")]
    [SerializeField] TextMeshProUGUI foodTimerText;
    /// <summary>
    /// Panel containing the list of foods to choose from
    /// </summary>
    [Tooltip("Look for 'Food List Panel' in the main canvas")]
    [SerializeField] GameObject foodListPanel;
    /// <summary>
    /// The canvas
    /// </summary>
    [SerializeField] GameObject applianceCanvas;
    /// <summary>
    /// The gameobject holding timer stuff
    /// </summary>
    [SerializeField] GameObject displayTimer;
    /// <summary>
    /// The gameobject called done display
    /// </summary>
    [SerializeField] GameObject doneDisplay;
    /// <summary>
    /// The prefab for displaying the ingredients
    /// </summary>
    [SerializeField] GameObject ingredientDisplayPrefab;
    /// <summary>
    /// The particle system
    /// </summary>
    [SerializeField] GameObject particleSystem;

    bool isCooking;
    [HideInInspector] public bool isDone;
    float timer;
    float cleanTimer;
    List<IngredientSO> ingredients;
    List<Image> ingredientDisplayList;

	// Use this for initialization
	void Start () {
        NewFood();
	}
	
	// Update is called once per frame
	void Update () {
        
        if (isCooking)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                float speed = 3.5f / selectedFood.timer;
                float step = speed * Time.deltaTime;
                foodTimerFront.rectTransform.localPosition = Vector3.MoveTowards(foodTimerFront.rectTransform.localPosition, Vector3.zero, step);
                foodTimerText.text = Mathf.CeilToInt(timer).ToString() + "s";
            }
            else
                IsDone();
        }
        //
        //if (cleanTimer > 0)
        //    cleanTimer -= Time.deltaTime;
        //else
        //    NewFood();

        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (IngredientSO ingredient in selectedFood.ingredientList)
                AddIngredient(ingredient);
        }

        if (Input.GetKeyUp(KeyCode.R))
            NewFood();
    }

    /// <summary>
    /// This adds the ingredient to the cooking appliance in preparation for cooking
    /// </summary>
    /// <param name="ingredient"></param>
    public void AddIngredient(IngredientSO ingredientSO)
    {
        if (!selectedFood)
            return;

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

            // Once there are all the needed ingredients
            if (ingredients.Count == selectedFood.ingredientList.Count)
                Cook();
        }
    }

    /// <summary>
    /// This is called when the player has finished cooking and the appliance is ready to be used again.
    /// </summary>
    public void NewFood()
    {
        isCooking = false;
        isDone = false;
        timer = 0;
        cleanTimer = 0;
        ingredients = new List<IngredientSO>();
        ingredientDisplayList = new List<Image>();
        selectedFood = null;

        OpenCloseCanvas(false);
        OpenCloseDoneDisplay(false);
        OpenCloseFoodMenu(false);
        OpenCloseIngredients(false);
        OpenCloseTimer(false);

        particleSystem.SetActive(false);
    }

    /// <summary>
    /// This happens when the player did not make any mistakes and the cooking timer hits zero
    /// i.e. the food is done cooking
    /// </summary>
    void IsDone()
    {
        isCooking = false;
        isDone = true;

        OpenCloseTimer(false);
        OpenCloseDoneDisplay(true);

        Image foodImage = doneDisplay.transform.GetChild(0).gameObject.GetComponent<Image>();
        foodImage.sprite = selectedFood.sprite;

        foodImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(2, 2);

        ResizeCanvas(2.5f, 2.5f);
        particleSystem.SetActive(false);
    }

    /// <summary>
    /// This happens when the player adds in the wrong ingredient.
    /// It resets all the ingredients in this cooking appliance.
    /// </summary>
    //void Failed()
    //{
    //    // Do Failed stuffs here
    //
    //    NewFood();
    //}

    public void ChooseFood(FoodSO foodSO)
    {
        //Debug.Log("Choosing food");

        if (selectedFood)
            return;

        selectedFood = foodSO;
        OpenCloseCanvas(true);
        OpenCloseFoodMenu(false);
        OpenCloseIngredients(true);

        // Do other stuff which happens when a food is selected
        foreach (IngredientSO ingredient in foodSO.ingredientList)
        {
            GameObject prefab = Instantiate(ingredientDisplayPrefab, ingredientPanel);
            Image ingredientImage = prefab.GetComponent<Image>();
            ingredientImage.sprite = ingredient.sprite;
            ingredientDisplayList.Add(ingredientImage);
        }

        ResizeCanvas(2.9f, 1.2f * Mathf.CeilToInt(foodSO.ingredientList.Count) / 2 + 0.5f);
    }

    //public void OpenList()
    //{
    //    if (foodListPanel.activeInHierarchy || selectedFood)
    //        return;
    //
    //    foodListPanel.SetActive(true);
    //
    //    foreach (FoodSO foodSO in foodList)
    //    {
    //        GameObject prefab = Instantiate(foodButtonPrefab, foodButtonPanel);
    //        TextMeshProUGUI foodName = prefab.GetComponentInChildren<TextMeshProUGUI>();
    //        foodName.text = foodSO.foodName;
    //        Image foodImage = prefab.GetComponentInChildren<Image>();
    //        foodImage.sprite = foodSO.sprite;
    //        Button foodButton = prefab.GetComponentInChildren<Button>();
    //        foodButton.onClick.AddListener(() => ChooseFood(foodSO));
    //    }
    //}

    /// <summary>
    /// Starts the cooking process and indicators
    /// </summary>
    public void Cook()
    {
        OpenCloseIngredients(false);
        OpenCloseTimer(true);

        ResizeCanvas(5.5f, 1.2f);
        particleSystem.SetActive(true);
    }

    /// <summary>
    /// True means open and false means close
    /// </summary>
    /// <param name="openclose"></param>
    public void OpenCloseIngredients(bool openclose)
    {
        foreach (Transform child in ingredientPanel)
            Destroy(child.gameObject);

        ingredientPanel.transform.parent.gameObject.SetActive(openclose);

        RadialMenu radialMenu = ingredientPanel.parent.gameObject.GetComponentInChildren<RadialMenu>();
        if (openclose && ingredientPanel.transform.parent.GetChild(1).childCount == 1)
            radialMenu.CallThisInsteadIngredient(7);

        if (radialMenu.currentFood)
            radialMenu.ChangeColor(radialMenu.currentFood);
        //ingredientPanel.gameObject.SetActive(openclose);
    }

    /// <summary>
    /// True means open and false means close
    /// </summary>
    /// <param name="openclose"></param>
    public void OpenCloseFoodMenu(bool openclose)
    {
        if (openclose)
        {
            if (selectedFood || foodListPanel.activeInHierarchy)
                return;

            foodListPanel.SetActive(openclose);

            foodButtonPanel.GetComponent<RadialMenu>().CallThisInsteadFood(this);
            //foreach (FoodSO foodSO in foodList)
            //{
                //GameObject prefab = Instantiate(radialMenuButtonPrefab, foodButtonPanel);
                //TextMeshProUGUI foodName = prefab.GetComponentInChildren<TextMeshProUGUI>();
                //foodName.text = foodSO.foodName;
                //Image foodImage = prefab.GetComponentInChildren<Image>();
                //foodImage.sprite = foodSO.sprite;
                //Button foodButton = prefab.GetComponentInChildren<Button>();
                //foodButton.onClick.AddListener(() => ChooseFood(foodSO));
            //}
        }
        else
        {
            List<Transform> children = foodButtonPanel.GetComponentsInChildren<Transform>().ToList();
            children.RemoveRange(0, 2);

            foreach (Transform child in children)
                Destroy(child.gameObject);
            
            foodListPanel.SetActive(openclose);
        }
    }

    public void OpenCloseTimer(bool openclose)
    {
        displayTimer.SetActive(openclose);

        if (openclose)
        {
            isCooking = true;
            timer = selectedFood.timer;
            cleanTimer = selectedFood.cleanTimer;
            foodDisplay.sprite = selectedFood.sprite;
            foodDisplay.preserveAspect = true;
            foodTimerFront.rectTransform.localPosition = new Vector3(-3.5f, 0);
            foodTimerText.text = Mathf.CeilToInt(timer).ToString();
        }
    }

    public void OpenCloseDoneDisplay(bool openclose)
    {
        doneDisplay.SetActive(openclose);
    }

    void ResizeCanvas(float x, float y)
    {
        applianceCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);
    }

    void OpenCloseCanvas(bool openclose)
    {
        applianceCanvas.SetActive(openclose);
    }

    /// <summary>
    /// Gets the current cooked food
    /// </summary>
    /// <returns></returns>
    public FoodSO TakeFood()
    {
        return selectedFood;
    }
}
