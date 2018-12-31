using System.Collections.Generic;
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
    private float endTime = 0.5f;

    private void Start()
    {      
        NuitrackManager.onHandsTrackerUpdate += NuitrackManager_onHandsTrackerUpdate;
        dragSensitivity *= dragSensitivity;
    }

    private void OnDestroy()
    {
        NuitrackManager.onHandsTrackerUpdate -= NuitrackManager_onHandsTrackerUpdate;
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
                }
                else if (currentHand == Hands.left && userHands.LeftHand != null)
                {                    
                    baseRect.anchoredPosition = new Vector2(userHands.LeftHand.Value.X * Screen.width, -userHands.LeftHand.Value.Y * Screen.height);
                    active = true;
                    press = userHands.LeftHand.Value.Click;

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
                }
            }
        }

        // Show Image
        background.enabled = active;
        background.sprite = active && press ? pressSprite : defaultSprite;

        if (!active)
            return;

        // Only Raycast to objects when food list panel is not active
        if (!foodListPanel.activeSelf)
        {
            RaycastHit hit;
            var landingRay = new Ray(transform.position, (transform.position - cam.transform.position).normalized);

            //// Draw ray in Scene view for Debug
            Debug.DrawRay(transform.position, (transform.position - cam.transform.position).normalized * 10f);

            // Raycast 10 units with landingRay
            if (Physics.Raycast(landingRay, out hit, 10f))
            {
                //// Check hit which object
                //Debug.Log(hit.transform.name);

                // Highlight Code Segment
                {
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
                    hitTransform = hit.transform;

                    // Enable highlight for hit object and its children
                    var outlines = hitTransform.GetComponentsInChildren<Outline>();
                    foreach (var outline in outlines)
                    {
                        outline.enabled = true;
                    }
                }

                if (press)
                {
                    elapsedTime += Time.deltaTime;

                    if (elapsedTime >= endTime)
                    {
                        // If selecting Cooking Appliance(Frying Pan, Pot 1, Pot 2)
                        if (LevelManager.Instance.cookingAppliances.Select(x => x.gameObject.GetInstanceID()).Contains(hit.transform.gameObject.GetInstanceID()))
                        {
                            var app = hit.transform.GetComponent<CookingAppliance>();

                            if (!app.isDone)
                            {
                                if (!ingredientSO)
                                {
                                    // Open up Food list to choose "food to cook"
                                    app.OpenCloseFoodMenu(true);
                                }
                                else
                                {
                                    // Add food(ingredient) into the appliance stated above
                                    app.AddIngredient(ingredientSO);

                                    // Disable highlight on selected ingredient
                                    var o = ingredient.GetComponentsInChildren<Outline>();
                                    foreach (var oL in o)
                                    {
                                        oL.selected = false;
                                        oL.color = 0;
                                    }
                                    ingredient = null;
                                    ingredientSO = null;
                                }
                            }
                            else
                            {
                                // If previously selected another cooking Appliance
                                if (cookingAppliance)
                                {
                                    // Disable highlight on selected ingredient
                                    var O = cookingAppliance.GetComponentsInChildren<Outline>();
                                    foreach (var oL in O)
                                    {
                                        oL.selected = false;
                                        oL.color = 0;
                                    }
                                }

                                // Select food and store it for serving customer later
                                cookingAppliance = hit.transform.gameObject;
                                foodSO = app.TakeFood();

                                // Enable highlight on selected cooking Appliance
                                var o = cookingAppliance.GetComponentsInChildren<Outline>();
                                foreach (var oL in o)
                                {
                                    oL.selected = true;
                                    oL.color = 1;
                                }
                            }
                        }
                        // If selecting ingredients(Banana Leaves, Tofus, Eggs, Noodle, Spice, Fishes, Prawns)
                        else if (LevelManager.Instance.ingredients.Select(x => x.gameObject.GetInstanceID()).Contains(hit.transform.gameObject.GetInstanceID()))
                        {
                            bool chosenFood = false;
                            if (LevelManager.Instance.cookingAppliances.Select(x => x.selectedFood).Distinct().Any())
                                chosenFood = true;

                            if (!chosenFood)
                                return;

                            // If previously selected another ingredient
                            if (ingredient)
                            {
                                // Disable highlight on selected ingredient
                                var O = ingredient.GetComponentsInChildren<Outline>();
                                foreach (var oL in O)
                                {
                                    oL.selected = false;
                                    oL.color = 0;
                                }
                                ingredient = null;
                                ingredientSO = null;
                            }

                            // Set ingredient
                            ingredient = hit.transform.gameObject;
                            ingredientSO = ingredient.GetComponent<Ingredient>().ingredientSO;

                            // Enable highlight on selected ingredient
                            var o = ingredient.GetComponentsInChildren<Outline>();
                            foreach (var oL in o)
                            {
                                oL.selected = true;
                                oL.color = 1;
                            }
                        }
                        // If selecting customer
                        else if (CustomerSpawner.Instance.customerDic.Select(x => x.Value.gameObject.GetInstanceID()).Contains(hit.transform.parent.gameObject.GetInstanceID()))
                        {
                            var customer = hit.transform.gameObject.GetComponentInParent<Customer>();

                            if (foodSO)
                            {
                                if (foodSO == customer.foodOrdered)
                                {
                                    // Served correct food, Add Score
                                    Score.instance.Profit(customer.foodOrdered);

                                    // Reset cooking Appliance status
                                    cookingAppliance.GetComponent<CookingAppliance>().NewFood();
                                }
                                else
                                {
                                    // Served wrong food, Decrease Rate
                                    Score.instance.Rate -= 0.1f;
                                }
                                // Customer leaves
                                customer.Leave(customer.customerId);

                                // Disable highlight on selected cooking Appliance
                                var o = cookingAppliance.GetComponentsInChildren<Outline>();
                                foreach (var oL in o)
                                {
                                    oL.selected = false;
                                    oL.color = 0;
                                }
                                cookingAppliance = null;
                                foodSO = null;
                            }
                        }
                        elapsedTime = 0f;
                    }
                }
            }
            else
            {
                // Disable highlight for every objects that have outline
                var o = FindObjectsOfType<Outline>();
                foreach (var oL in o)
                {
                    if (!oL.selected)
                    {
                        oL.enabled = false;
                    }
                }
            }
        }

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
                selectedButton.OnPointerExit(eventData);
        
            selectedButton = newButton;
        
            if (selectedButton != null)
                selectedButton.OnPointerEnter(eventData);
        }
        else if (selectedButton != null)
        {
            if (press)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= endTime)
                {
                    //if (eventData.delta.sqrMagnitude < dragSensitivity && !eventData.dragging)
                    //{
                    //eventData.dragging = true;
                    selectedButton.OnPointerDown(eventData);
                    selectedButton.OnPointerClick(eventData);
                    //}

                    //// Shoot bullet towards hand icon
                    //GameObject Projectile = ObjectPool.instance.GetPooledObject(ProjectilePrefab);

                    //if (!Projectile) return;

                    //Projectile.transform.position = hand.position;
                    //Projectile.transform.rotation = Quaternion.identity;
                    //Projectile.GetComponent<Projectile>().dir = (background.transform.position - hand.position).normalized;
                    
                    elapsedTime = 0f;
                }
            }
            //else if (eventData.dragging)
            //{
                //eventData.dragging = false;
                selectedButton.OnPointerUp(eventData);
            //}

            //selectedButton.OnDrag(eventData);
        }
    }
}
