using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// This class manages everything about Hand Pointer and Player Controls.
/// 
/// Can be found attached in Canvas -> LHand and RHand
/// </summary>
public class HandPointer : MonoBehaviour
{
    /// <summary>
    /// Enumeration to determine this is left or right hand pointer.
    /// </summary>
    public enum Hands
    {
        left = 0,
        right = 1
    };
    
    /// <summary>
    /// Knife Projectile Prefab for player to throw.
    /// </summary>
    [Tooltip("Knife Projectile Prefab for player to throw.\n" +
    "\n" +
    "Can be found in Assets -> Prefabs -> Projectile")]
    [SerializeField] private GameObject ProjectilePrefab;

    [Header ("Visualization")]
    [Space(5)]

    /// <summary>
    /// Open Hand Pointer Sprite.
    /// </summary>
    [Tooltip("Open Hand Pointer Sprite.\n" +
        "\n" +
        "Can be found in Assets -> UI")]
    [SerializeField] private Sprite defaultSprite;

    /// <summary>
    /// Closed Hand Pointer Sprite.
    /// </summary>
    [Tooltip("Closed Hand Pointer Sprite.\n" +
    "\n" +
    "Can be found in Assets -> UI")]
    [SerializeField] private Sprite pressSprite;
    
    [Space(5)]
    [Header("References to Menus")]
    [Space(5)]

    /// <summary>
    /// Food List Panel(A menu used to select the food to cook).
    /// </summary>
    [Tooltip("Food List Panel.\n" +
        "Can be found in Canvas")]
    [SerializeField] private GameObject foodListPanel;

    /// <summary>
    /// Ingredient List Panel(A menu used to select the ingredient to cook).
    /// </summary>
    [Tooltip("Ingredient List Panel.\n" +
        "Can be found in Canvas")]
    [SerializeField] private GameObject ingredientListPanel;

    [Space(5)]
    [Header("Hand Pointer Data")]
    [Space(5)]

    /// <summary>
    /// Enumeration to determine this is left or right hand pointer.
    /// </summary>
    [Tooltip("Enumeration to determine this is left or right hand pointer.")]
    public Hands currentHand;

    /// <summary>
    /// A float that determines the time to trigger the button.
    /// 
    /// Default: 0.5
    /// </summary>
    [Tooltip("A float that determines the time to trigger the button.\n" +
        "Default: 0.5")]
    [SerializeField] private float handTimer = 0.5f;

    /// <summary>
    /// A float that determines the time to throw knife.
    /// 
    /// Defualt: 0.15
    /// </summary>
    [Tooltip("A float that determines the time to throw knife.\n" +
        "Default: 0.15")]
    [SerializeField] private float shootingTimer = 0.15f;

    /// <summary>
    /// A float that determines how big the circle is, for the flower image to move according to.
    /// 
    /// Default: 80
    /// </summary>
    [Tooltip("A float that determines how big the circle is, for the flower image to move according to.\n" +
        "\n" +
        "Default: 80")]
    [SerializeField] private float radius = 80f;

    /// <summary>
    /// RectTransform Component of this GameObject.
    /// </summary>
    private RectTransform baseRect;

    /// <summary>
    /// Image Component of this GameObject.
    /// </summary>
    private Image background;

    /// <summary>
    /// A boolean that determine if this GameObject is active.
    /// </summary>
    private bool active = false;

    /// <summary>
    /// A boolean that determine if Player is performing Grab Gesture.
    /// </summary>
    private bool press = false;
    
    /// <summary>
    /// Main Camera in the game.
    /// </summary>
    private Camera cam;

    /// <summary>
    /// The button this GameObject is currently triggering.
    /// </summary>
    private Button selectedButton;

    /// <summary>
    /// Raycasting variable.
    /// </summary>
    private PointerEventData eventData = new PointerEventData(null);
    
    /// <summary>
    /// List of Raycast Results.
    /// </summary>
    private List<RaycastResult> raycastResults = new List<RaycastResult>();

    /// <summary>
    /// A Transform that stores previously Highlighted GameObject.
    /// </summary>
    private Transform hitTransform;

    /// <summary>
    /// A FoodSO that stores Grabbed food.
    /// </summary>
    private FoodSO foodSO;

    /// <summary>
    /// A Cooking Appliance that stores a reference to which Appliance did this GameObject Grabbed from.
    /// </summary>
    private GameObject cookingAppliance;

    /// <summary>
    /// A float timer used to Trigger button by handTimer above
    /// </summary>
    private float handElapsedTime;

    /// <summary>
    /// A float timer used to Throw Knife by shootingTimer above
    /// </summary>
    private float shootingElapsedTime;

    /// <summary>
    /// An Image Component of Frame image of this GameObject.
    /// </summary>
    private Image timerImage;
    
    /// <summary>
    /// An Image Component of Flower image of this GameObject
    /// </summary>
    private Image timerImage2;

    /// <summary>
    /// A float that determines how big the circle is, for the flower image to move according to.
    /// 
    /// Default: 0
    /// </summary>
    private float angle = 0f;

    private void Awake()
    {
        baseRect = GetComponent<RectTransform>();
        background = GetComponent<Image>();
        timerImage = transform.GetChild(0).GetComponent<Image>();
        timerImage2 = transform.GetChild(1).GetComponent<Image>();
    }

    private void Start()
    {      
        NuitrackManager.onHandsTrackerUpdate += NuitrackManager_onHandsTrackerUpdate;

        // Initialise this GameObject's sprite
        background.sprite = defaultSprite;

        cam = Player.Instance.transform.GetChild(0).GetComponent<Camera>();
    }

    private void OnDestroy()
    {
        NuitrackManager.onHandsTrackerUpdate -= NuitrackManager_onHandsTrackerUpdate;
    }

    /// <summary>
    /// Drops currently held food, Change Sprite back to Hand, Reset references to cooking appliance and food.
    /// </summary>
    /// <param name="item"> The Item to Drop. </param>
    public void DropItem(GameObject item)
    {
        // Change Ingredient Sprite back to Hand Sprite
        background.sprite = defaultSprite;

        // If item is Null, do not do anything
        if (item == null)
        {
            return;
        }

        // Disable highlight on selected ingredient
        var o = item.GetComponentsInChildren<Outline>();
        foreach (var oL in o)
        {
            oL.selected = false;
            oL.color = 0;
        }

        if (item == cookingAppliance)
        {
            // Reset Food
            cookingAppliance = null;
            foodSO = null;
        }
    }

    /// <summary>
    /// Highlight the passed in GameObject.
    /// </summary>
    /// <param name="parent"> THe GameObject to Highlight. </param>
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

        // Set Hand Pointer Data if can detect hand
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
                }
                else if (currentHand == Hands.left && userHands.LeftHand != null)
                {                    
                    baseRect.anchoredPosition = new Vector2(userHands.LeftHand.Value.X * Screen.width, -userHands.LeftHand.Value.Y * Screen.height);
                    active = true;
                    press = userHands.LeftHand.Value.Click;
                }
            }
        }

        // Show Image
        background.enabled = active;

        if(active)
        {
            // Change back to Hand Sprite if not holding food
            if(!foodSO)
            {
                background.sprite = defaultSprite;
            }
        }
        else
        {
            // Do not do anything if not active
            return;
        }

        // Raycast from Screen Space to World Space
        var pointOnScreenPosition = (Vector2)cam.WorldToScreenPoint(transform.position);
        eventData.delta = pointOnScreenPosition - eventData.position;
        eventData.position = pointOnScreenPosition;

        raycastResults.Clear();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        Button newButton = null;

        for (int i = 0; i < raycastResults.Count && newButton == null; i++)
            newButton = raycastResults[i].gameObject.GetComponent<Button>();
        
        if (newButton != selectedButton)
        {
            // When current selected button is not previous sselected button
            if (selectedButton != null)
            {
                selectedButton.OnPointerExit(eventData);

                // Reset Hand Timer, Frame and Flower
                handElapsedTime = 0f;
                timerImage.fillAmount = 0f;
                timerImage2.gameObject.SetActive(false);
                angle = -(timerImage.fillAmount * 360f + 90f) * Mathf.Deg2Rad;
                var offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                timerImage2.transform.localPosition = new Vector3(offset.x, offset.y, 0f);

                // If previous selected button is Ingredient Panel, stop zooming
                if (selectedButton.GetComponent<IngredientPanel>())
                    selectedButton.GetComponent<IngredientPanel>().Zoomasaurus(false);

                // If any of the cooking appliance's hover hint is active, close it
                if (LevelManager.Instance.cookingAppliances.Any(x => x.hoverHint.activeSelf))
                {
                    foreach (var app in LevelManager.Instance.cookingAppliances)
                    {
                        if (app.hoverHint.activeSelf)
                        {
                            app.OpenCloseHint(false);
                            app.OpenCloseCanvas(false);
                        }
                    }
                }
            }
        
            // Updates selected buttpon
            selectedButton = newButton;
        
            if (selectedButton != null)
            {
                selectedButton.OnPointerEnter(eventData);

                // If selected button is Ingredient Panel, start zooming
                if (selectedButton.GetComponent<IngredientPanel>())
                    selectedButton.GetComponent<IngredientPanel>().Zoomasaurus(true);
            }
        }
        else if (selectedButton != null)
        {
            // Runs timer
            handElapsedTime += Time.deltaTime;

            // Reduce fillAmount of Timer Filler Image(visual feedback) over waitTiming
            timerImage.fillAmount += (1f / handTimer) * Time.deltaTime;

            // Shows Flower Image and Move it in a circle
            timerImage2.gameObject.SetActive(true);
            angle = -(timerImage.fillAmount * 360f + 90f) * Mathf.Deg2Rad;
            var offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            timerImage2.transform.localPosition = new Vector3(offset.x, offset.y, 0f);

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

            // When the game is not paused, menus not active and selected button is not pause nor guide image
            if (PauseManager.Instance != null && !PauseManager.Instance.isPaused && !foodListPanel.activeSelf && !ingredientListPanel.activeSelf
                && selectedButton.name != "Pause" && selectedButton.name != "GuideImage")
            {
                // If selecting Cooking Appliance(Frying Pan, Pot 1, Pot 2)
                if (LevelManager.Instance.cookingAppliances.Any(x => x.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()))
                {
                    var something = LevelManager.Instance.cookingAppliances.Where(x => x.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()).ToList();

                    if (something.Count != 1)
                        return;

                    var app = something[0].GetComponent<CookingAppliance>();
                    
                    app.OpenCloseHint(true);
                    app.OpenCloseCanvas(true);
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

            // When hand timer reach limit
            if (handElapsedTime >= handTimer)
            {
                // Reset Hand Timer, Frame, and Flower image
                handElapsedTime = 0f;
                timerImage.fillAmount = 0f;
                timerImage2.gameObject.SetActive(false);
                angle = -(timerImage.fillAmount * 360f + 90f) * Mathf.Deg2Rad;
                var offset1 = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                timerImage2.transform.localPosition = new Vector3(offset1.x, offset1.y, 0f);
                
                selectedButton.OnPointerClick(eventData);

                // When the game is not paused, menus not active and selected button is not pause
                if (PauseManager.Instance != null && !PauseManager.Instance.isPaused && !foodListPanel.activeSelf && !ingredientListPanel.activeSelf
                    && selectedButton.name != "Pause")
                {
                    // Do not do anything if haven't finish guiding about intro and order
                    if (selectedButton.name == "GuideImage")
                    {
                        if (!selectedButton.GetComponent<Guide>().finishedIntro || !selectedButton.GetComponent<Guide>().finishedOrder)
                        {
                            return;
                        }
                    }

                    // If selecting Cooking Appliance(Frying Pan, Pot 1, Pot 2)
                    else if (LevelManager.Instance.cookingAppliances.Any(x => x.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()))
                    {
                        var something = LevelManager.Instance.cookingAppliances.Where(x => x.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()).ToList();

                        if (something.Count != 1)
                            return;

                        var app = something[0].GetComponent<CookingAppliance>();

                        // Haven't done cooking food
                        if (!app.isDone)
                        {
                            // Open up Food list to choose "food to cook"
                            app.OpenCloseFoodMenu(true);

                            // If the game is in Tutorial Scene and Guide Image is not active
                            if (Menu_Manager.Instance.Tutorial_Mode && !Guide.Instance.gameObject.activeSelf)
                            {
                                // If haven't guide cook, start guiding
                                if(!Guide.Instance.CheckIfGuidedCook())
                                {
                                    Guide.Instance.Show();
                                }
                            }
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

                            // Allow interactions with customer when holding food
                            foreach(var pair in CustomerSpawner.Instance.customerDic)
                            {
                                pair.Value.AllowHover(true);
                            }

                            // Change Hand Sprite to Food Sprite
                            background.sprite = foodSO.sprite;
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

                            // When customer is not fighting
                            if (!customer.fighting)
                            {
                                // Serve Correct
                                if (foodSO == customer.foodOrdered)
                                {
                                    // Set customer's animations and sound effect
                                    customer.SetAnim(customer.idle, false);
                                    customer.SetAnim(customer.happy, true);
                                    customer.SetClip(Audio_Manager.Instance.audioDictionary["Coin Drop"]);

                                    // Served correct food, Add Score
                                    Score.Instance.Profit(customer.foodOrdered, customer.timerImage.fillAmount);

                                    // Customer leaves
                                    customer.Leave();
                                }
                                // Serve Wrong
                                else
                                {
                                    // Set customer's animations
                                    customer.SetAnim(customer.idle, false);
                                    customer.SetAnim(customer.angry, true);

                                    // Served wrong food, Decrease Rate
                                    Score.Instance.rate -= 0.1f;
                                    customer.fighting = true;

                                    foreach (CookingAppliance appliance in LevelManager.Instance.cookingAppliances)
                                        if (!appliance.isDone)
                                            appliance.NewFood();

                                    customer.player = Player.Instance.transform;

                                    // If the game is in Tutorial Scene
                                    if (Guide.Instance != null)
                                    {
                                        Guide.Instance.gameObject.SetActive(true);
                                    }
                                }

                                // Disable interactions with customer
                                foreach(var pair in CustomerSpawner.Instance.customerDic)
                                {
                                    pair.Value.AllowHover(false);
                                }

                                // Reset cooking Appliance status
                                CookingAppliance app = cookingAppliance.GetComponent<CookingAppliance>();
                                app.NewFood();

                                // Drop food
                                DropItem(cookingAppliance);
                            }
                        }
                    }
                }
            }
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

        // If any customer is fighting with player
        if (CustomerSpawner.Instance.customerDic.Any(x => x.Value.fighting == true))
        {
            // If the game is in Pause Status, do not do anything
            if (PauseManager.Instance != null && PauseManager.Instance.isPaused)
            {
                return;
            }

            var something = CustomerSpawner.Instance.customerDic.Where(x => x.Value.fighting == true).ToList();
            
            foreach(var pair in something)
            {
                var customer = pair.Value.GetComponent<Customer>();

                // Set player transform for customer to have a target to shoot at
                if (!customer.player)
                {
                    customer.player = Player.Instance.transform;
                }
            }            

            // If menus are inactive and player is performing Grab Gesture
            if (!foodListPanel.activeSelf && !ingredientListPanel.activeSelf && press)
            {
                // Runs timer
                shootingElapsedTime += Time.deltaTime;

                // When timer reach limit
                if (shootingElapsedTime >= shootingTimer)
                {
                    // Reset timer
                    shootingElapsedTime = 0f;

                    // Shoot bullet towards hand icon
                    GameObject Projectile = ObjectPool.Instance.GetPooledObject(ProjectilePrefab);

                    // If projectile is not Null
                    if (!Projectile)
                    {
                        return;
                    }

                    // Initialise position and direction of projectile
                    Projectile.transform.position = transform.position;
                    Projectile.GetComponent<Projectile>().dir = (transform.position - cam.transform.position).normalized;
                }
            }
        }
    }
}