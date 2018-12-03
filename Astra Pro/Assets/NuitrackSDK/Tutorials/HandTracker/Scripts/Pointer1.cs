﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    public Transform hand;

    ImageItem selectedButton;

    PointerEventData eventData = new PointerEventData(null);
    List<RaycastResult> raycastResults = new List<RaycastResult>();

    [SerializeField]
    float dragSensitivity = 5f;

    [SerializeField] private GameObject ProjectilePrefab;

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

        // Show Image
        background.enabled = active;
        background.sprite = active && press ? pressSprite : defaultSprite;

        if (!active)
            return;


        Vector2 pointOnScreenPosition = cam.WorldToScreenPoint(transform.position);
        eventData.delta = pointOnScreenPosition - eventData.position;
        eventData.position = pointOnScreenPosition;

        raycastResults.Clear();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        ImageItem newButton = null;

        for (int i = 0; i < raycastResults.Count && newButton == null; i++)
            newButton = raycastResults[i].gameObject.GetComponent<ImageItem>();

        // Calls event override functions in imageitem1.cs
        //if (newButton != selectedButton)
        //{
        //    if (selectedButton != null)
        //        selectedButton.OnPointerExit(eventData);
        //
        //    selectedButton = newButton;
        //
        //    if (selectedButton != null)
        //        selectedButton.OnPointerEnter(eventData);
        //}
        //else if (selectedButton != null)
        {
            if (press)
            {
                //if (eventData.delta.sqrMagnitude < dragSensitivity && !eventData.dragging)
                //{
                //    eventData.dragging = true;
                //    selectedButton.OnPointerDown(eventData);
                //}

                // Shoot bullet towards hand icon
                GameObject Projectile = Instantiate(ProjectilePrefab, hand.position, Quaternion.identity, hand);
                Projectile.GetComponent<Projectile>().dir = (background.transform.position - hand.position).normalized;
                Destroy(Projectile, 2);
            }
           //else if (eventData.dragging)
           //{
           //    eventData.dragging = false;
           //    selectedButton.OnPointerUp(eventData);
           //}

            //if (press)
            //    Menu_Manager.OnGameMenu();
                //selectedButton.OnDrag(eventData);
        }
    }
}
