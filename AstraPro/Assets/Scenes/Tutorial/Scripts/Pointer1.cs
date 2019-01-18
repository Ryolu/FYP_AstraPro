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
    [SerializeField] private GameObject ingredientListPanel;
    [SerializeField] private float handTimer = 0.75f;
    [SerializeField] private float shootingTimer = 0.15f;

    private Transform hitTransform;
    private FoodSO foodSO;
    private GameObject cookingAppliance;
    private float handElapsedTime;
    private float shootingElapsedTime;
    private Image timerImage;

    // Flower
    private Image timerImage2;
    [SerializeField] private float radius = 80f;
    private float angle = 0f;

    private void Start()
    {      
        NuitrackManager.onHandsTrackerUpdate += NuitrackManager_onHandsTrackerUpdate;
        dragSensitivity *= dragSensitivity;
        background.sprite = defaultSprite;
        timerImage = transform.GetChild(0).GetComponent<Image>();
        timerImage2 = transform.GetChild(1).GetComponent<Image>();
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

        if (item == cookingAppliance)
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
            if(!foodSO)
            {
                background.sprite = defaultSprite;
            }
        }
        else
        {
            return;
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
            {
                selectedButton.OnPointerExit(eventData);
                handElapsedTime = 0f;
                timerImage.fillAmount = 0f;

                timerImage2.gameObject.SetActive(false);
                angle = -(timerImage.fillAmount * 360f + 90f) * Mathf.Deg2Rad;
                var offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                timerImage2.transform.localPosition = new Vector3(offset.x, offset.y, 0f);

                if (selectedButton.GetComponent<IngredientPanel>())
                    selectedButton.GetComponent<IngredientPanel>().Zoomasaurus(false);
            }
        
            selectedButton = newButton;
        
            if (selectedButton != null)
            {
                selectedButton.OnPointerEnter(eventData);
                if (selectedButton.GetComponent<IngredientPanel>())
                    selectedButton.GetComponent<IngredientPanel>().Zoomasaurus(true);
            }
        }
        else if (selectedButton != null)
        {
            handElapsedTime += Time.deltaTime;

            // Reduce fillAmount of Timer Filler Image(visual feedback) over waitTiming
            timerImage.fillAmount += (1f / handTimer) * Time.deltaTime;

            timerImage2.gameObject.SetActive(true);
            angle = -(timerImage.fillAmount * 360f + 90f) * Mathf.Deg2Rad;
            var offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            timerImage2.transform.localPosition = new Vector3(offset.x, offset.y, 0f);
            //Debug.Log("Fill: " + timerImage.fillAmount + " Angle: " + angle + " Center: " + center + " Offset: " + offset + " Result: " + result + " pos: " + timerImage2.transform.position);

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

                    hitTransform = app.transform;
                    ShowOutline(app.gameObject);
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

            if (handElapsedTime >= handTimer)
            {
                handElapsedTime = 0f;
                timerImage.fillAmount = 0f;

                timerImage2.gameObject.SetActive(false);
                angle = -(timerImage.fillAmount * 360f + 90f) * Mathf.Deg2Rad;
                var offset1 = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                timerImage2.transform.localPosition = new Vector3(offset1.x, offset1.y, 0f);
                
                // Call Button OnClick()
                //if (selectedButton.transform.parent.gameObject.GetComponent<RadialMenu>())
                //    Debug.Log("Clicked1");
                selectedButton.OnPointerClick(eventData);
                //if (selectedButton.transform.parent.gameObject.GetComponent<RadialMenu>())
                //    Debug.Log("Clicked2");

                if (PauseManager.Instance != null && !PauseManager.Instance.isPaused && !foodListPanel.activeSelf && !ingredientListPanel.activeSelf
                    && selectedButton.name != "Pause")
                {
                    if (selectedButton.name == "GuideImage")
                    {
                        if (!selectedButton.GetComponent<Guide>().finishedIntro || !selectedButton.GetComponent<Guide>().finishedOrder)
                        {
                            return;
                        }
                    }

                    // If selecting Cooking Appliance(Frying Pan, Pot 1, Pot 2)
                    if (LevelManager.Instance.cookingAppliances.Any(x => x.gameObject.GetInstanceID() == selectedButton.transform.parent.parent.gameObject.GetInstanceID()))
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

                            if (Menu_Manager.Instance.Tutorial_Mode == true)
                            {
                                Guide.Instance.gameObject.SetActive(true);
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
                                    customer.SetAnim(customer.idle, false);
                                    customer.SetAnim(customer.happy, true);
                                    // Served correct food, Add Score
                                    Score.Instance.Profit(customer.foodOrdered, customer.timerImage.fillAmount);

                                    // Customer leaves
                                    customer.Leave();
                                }
                                else
                                {
                                    customer.SetAnim(customer.idle, false);
                                    customer.SetAnim(customer.angry, true);
                                    // Served wrong food, Decrease Rate
                                    Score.Instance.rate -= 0.1f;
                                    customer.fighting = true;

                                    customer.player = cam.transform.parent;

                                    if (Guide.Instance != null)
                                    {
                                        Guide.Instance.gameObject.SetActive(true);
                                    }
                                }

                                // Reset cooking Appliance status
                                cookingAppliance.GetComponent<CookingAppliance>().NewFood();

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

        if (CustomerSpawner.Instance.customerDic.Any(x => x.Value.fighting == true))
        {
            if (PauseManager.Instance != null && PauseManager.Instance.isPaused) return;

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

            if (!foodListPanel.activeSelf && !ingredientListPanel.activeSelf && press)
            {
                shootingElapsedTime += Time.deltaTime;

                if (shootingElapsedTime >= shootingTimer)
                {
                    shootingElapsedTime = 0f;

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
}
