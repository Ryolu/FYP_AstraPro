using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hand : MonoBehaviour {

    public enum Hands { left = 0, right = 1 };

    [SerializeField]
    Hands currentHand;

    [Header("Visualization")]
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

    ImageItem selectedButton;

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
    }
}
