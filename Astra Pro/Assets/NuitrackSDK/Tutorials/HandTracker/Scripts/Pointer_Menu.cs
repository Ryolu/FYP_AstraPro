using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pointer_Menu : MonoBehaviour
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

    GameObject selectedButton;

    PointerEventData eventData = new PointerEventData(null);
    List<RaycastResult> raycastResults = new List<RaycastResult>();

    [SerializeField]
    float dragSensitivity = 5f;

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
                }
                else if (currentHand == Hands.left && userHands.LeftHand != null)
                {                    
                    baseRect.anchoredPosition = new Vector2(userHands.LeftHand.Value.X * Screen.width, -userHands.LeftHand.Value.Y * Screen.height);
                    active = true;
                    press = userHands.LeftHand.Value.Click;
                }
            }
        }

        background.enabled = active;
        background.sprite = active && press ? pressSprite : defaultSprite;
        
        if (!active)
            return;

        Vector2 pointOnScreenPosition = cam.WorldToScreenPoint(transform.position);
        eventData.delta = pointOnScreenPosition - eventData.position;
        eventData.position = pointOnScreenPosition;

        raycastResults.Clear();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        //ImageItem newButton = null;

        //for (int i = 0; i < raycastResults.Count && newButton == null; i++)
        //    newButton = raycastResults[i].gameObject.GetComponent<ImageItem>();
        
        GameObject newButton = null;

        for (int i = 0; i < raycastResults.Count && newButton == null; i++)
        {
            if (raycastResults[i].gameObject.GetComponent<ImageItem>())
            {
                newButton = raycastResults[i].gameObject;
                //Debug.Log("Hit");
            }
        }

        if (newButton != selectedButton)
        {
            if (selectedButton != null)
            {
                selectedButton.GetComponent<ImageItem>().OnPointerExit(eventData);
                Debug.Log("Exit");
            }

            selectedButton = newButton;

            if (selectedButton != null)
            {
                selectedButton.GetComponent<ImageItem>().OnPointerEnter(eventData);
                Debug.Log("Enter");
            }
        }
        else if (selectedButton != null)
        {
            if (press)
            {
                if (eventData.delta.sqrMagnitude < dragSensitivity && !eventData.dragging)
                {
                    eventData.dragging = true;
                    selectedButton.GetComponent<ImageItem>().OnPointerDown(eventData);
                }
            }
            else if (eventData.dragging)
            {
                eventData.dragging = false;
                selectedButton.GetComponent<ImageItem>().OnPointerUp(eventData);
            }

            if (press)
            {
                if (selectedButton.name == "Start" || selectedButton.name == "Return_MainMenu")
                {
                    Menu_Manager.OnGameMenu();
                }
                else if (selectedButton.name == "SFX_Check" || selectedButton.name == "BGM_Check")
                {
                    if (selectedButton.GetComponentInParent<Toggle>().isOn == true)
                    {
                        selectedButton.GetComponentInParent<Toggle>().isOn = false;

                        Debug.Log("Off");
                    }
                    else
                    {
                        selectedButton.GetComponentInParent<Toggle>().isOn = true;
                        Debug.Log("On");
                    }
                    //selectedButton.OnDrag(eventData);
                }
                //else if (selectedButton.name == "Handle_2")
                //{
                //    if (selectedButton.GetComponentInParent<Slider>())
                //    {
                //        selectedButton.GetComponent<ImageItem>().interactable = false;
                //        selectedButton.GetComponent<ImageItem>().OnDrag(eventData);
                //        Debug.Log("handleeeeeeeeeeeeeeeeeeeeeeeee");
                //    }
                //}
                else
                    selectedButton.GetComponent<ImageItem>().OnDrag(eventData);
            }
        }
    }
}
