using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class CookingAppliance : MonoBehaviour {

    [Header("Appliance Data")]
    [Space(5)]
    /// <summary>
    /// The list of foods which can be cooked by this appliance
    /// </summary>
    public List<FoodSO> foodList;
    /// <summary>
    /// Currently selected food
    /// </summary>
    [HideInInspector]
    public FoodSO selectedFood;
    [Space(10)]

    [Header("Main Canvas Objects")]
    [Space(5)]
    /// <summary>
    /// The transform to display the buttons which determine which food will be cooked
    /// This panel can be found in the main canvas
    /// </summary>
    [Tooltip("This panel can be found in the main canvas")]
    [SerializeField] Transform foodButtonPanel;
    /// <summary>
    /// The transform to display the ingredients left to put into this appliance
    /// </summary>
    [Tooltip("This panel can be found in the main canvas")]
    [SerializeField] Transform ingredientPanel;
    /// <summary>
    /// Panel containing the list of foods to choose from
    /// </summary>
    [Tooltip("Look for 'Food List Panel' in the main canvas")]
    [SerializeField] GameObject foodListPanel;

    [Space(10)]

    [Header("Appliance-specific Objects")]
    [Space(5)]
    /// <summary>
    /// The image which displays which food is being cooked
    /// </summary>
    [Tooltip("This image can be found in the canvas attached to this appliance under the Display Timer Panel")]
    [SerializeField] Image foodDisplay;

    [Tooltip("This image can be found in the canvas attached to this appliance under the Display Timer Panel")]
    [SerializeField] Image foodTimerFront;

    [Tooltip("This text can be found in the canvas attached to this appliance under the Display Timer Panel")]
    [SerializeField] TextMeshProUGUI foodTimerText;
    /// <summary>
    /// The canvas
    /// </summary>
    [Tooltip("Literally the first immediate child of this GameObject")]
    [SerializeField] GameObject applianceCanvas;
    /// <summary>
    /// The hints for what food this appliance can cook
    /// </summary>
    [Tooltip("Child of applianceCanvas")]
    public GameObject hoverHint;
    /// <summary>
    /// The gameobject holding timer stuff
    /// </summary>
    [Tooltip("Child of applianceCanvas")]
    [SerializeField] GameObject displayTimer;
    /// <summary>
    /// The gameobject called done display
    /// </summary>
    [Tooltip("Child of applianceCanvas")]
    [SerializeField] GameObject doneDisplay;
    /// <summary>
    /// The visual feedback for starting cooking process
    /// </summary>
    [Tooltip("Literally the third immediate child of this GameObject")]
    [SerializeField] GameObject cookingModel;
    [Space(10)]
    /// <summary>
    /// The particle system
    /// </summary>
    [Tooltip("This can be found under the Particle System empty GameObject")]
    [SerializeField] GameObject particleSystem;

    [Header("Prefabs")]
    [Space(5)]
    /// <summary>
    /// The prefab for displaying the ingredients
    /// </summary>
    [Tooltip("This can be found in gesture project's Prefabs folder, literally named the same")]
    [SerializeField] GameObject ingredientDisplayPrefab;
    /// <summary>
    /// The prefab for displaying the available foods
    /// </summary>
    [Tooltip("This can be found in gesture project's Prefabs folder, literally named the same")]
    [SerializeField] GameObject hoverHintPrefab;

    [HideInInspector] public bool isDone;
    bool isCooking;
    bool changeThisOnce;
    float timer;
    float cleanTimer;
    List<IngredientSO> ingredients;
    List<Image> ingredientDisplayList;

	// Use this for initialization
	void Start () {
        changeThisOnce = false;
        NewFood();

        for (int i = 0; i < foodList.Count; i++)
        {
            GameObject temp = Instantiate(hoverHintPrefab, hoverHint.transform);
            Image foodImage = hoverHint.transform.GetChild(i).gameObject.GetComponent<Image>();
            foodImage.sprite = foodList[i].sprite;

            foodImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1.5f, 1.5f);
        }
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

                if (foodList.Count > 1)
                    cookingModel.transform.localPosition += new Vector3(0, (0.01f + 0.0025f) / selectedFood.timer * Time.deltaTime, 0);
            }
            else
                IsDone();
        }

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
        OpenCloseHint(false);

        particleSystem.SetActive(false);
        ShowProcess(false);
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

        if (!changeThisOnce)
        {
            foodImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(2, 2);
            changeThisOnce = true;
        }

        ResizeCanvas(2.5f, 2.5f);
        particleSystem.SetActive(false);
    }
    
    /// <summary>
    /// Selects the food the player chooses as the food to be cooked
    /// </summary>
    /// <param name="foodSO"></param>
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

        //RadialMenu radialMenu = ingredientPanel.parent.gameObject.GetComponentInChildren<RadialMenu>();
        //if (radialMenu.currentFood)
        //    radialMenu.ChangeColor(radialMenu.currentFood);

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
        ShowProcess(true);
        if (foodList.Count > 1)
            cookingModel.transform.localPosition = new Vector3(cookingModel.transform.localPosition.x, -0.0025f, cookingModel.transform.localPosition.z);
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
        if (openclose)
            radialMenu.CallThisInsteadIngredient(7);
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

    /// <summary>
    /// True means open and false means close
    /// </summary>
    /// <param name="openclose"></param>
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

    /// <summary>
    /// True means open and false means close
    /// </summary>
    /// <param name="openclose"></param>
    public void OpenCloseDoneDisplay(bool openclose)
    {
        doneDisplay.SetActive(openclose);
    }

    /// <summary>
    /// True means open and false means close
    /// </summary>
    /// <param name="openclose"></param>
    public void OpenCloseHint(bool openclose)
    {
        if (selectedFood)
            return;

        hoverHint.SetActive(openclose);
        OpenCloseCanvas(openclose);

        if (openclose)
            ResizeCanvas(foodList.Count * 1.5f + 0.5f, 2.5f);
    }

    /// <summary>
    /// True means open and false means close
    /// </summary>
    /// <param name="openclose"></param>
    public void OpenCloseCanvas(bool openclose)
    {
        applianceCanvas.SetActive(openclose);
    }

    /// <summary>
    /// Wrapper for resizing the canvas
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void ResizeCanvas(float x, float y)
    {
        applianceCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);
    }

    /// <summary>
    /// Gets the current cooked food
    /// </summary>
    /// <returns></returns>
    public FoodSO TakeFood()
    {
        return selectedFood;
    }

    public void ShowProcess(bool show, bool putBack = false)
    {
        cookingModel.SetActive(show);

        if(show && !putBack)
            if (foodList.Count > 1)
                cookingModel.transform.localPosition = new Vector3(cookingModel.transform.localPosition.x, -0.0025f, cookingModel.transform.localPosition.z);
    }
}
