using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pointer_Menu : MonoBehaviour
{
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

    GameObject selectedButton;

    PointerEventData eventData = new PointerEventData(null);
    List<RaycastResult> raycastResults = new List<RaycastResult>();

    [SerializeField]
    float dragSensitivity = 5f;

    float ElapsedTime;
    float EndTime = 0.5f;
    float TemptBGM, TemptSFX;


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
            //if (press)
            //{
            //    if (eventData.delta.sqrMagnitude < dragSensitivity && !eventData.dragging)
            //    {
            //        eventData.dragging = true;
            //        selectedButton.GetComponent<ImageItem>().OnPointerDown(eventData);
            //    }
            //}
            //else if (eventData.dragging)
            //{
            //    eventData.dragging = false;
            //    selectedButton.GetComponent<ImageItem>().OnPointerUp(eventData);
            //}

            if (press)
            {
                if (selectedButton.name == "Start")
                {
                    Menu_Manager.Tutorial_Mode = false;
                    Menu_Manager.Instance.In_Game();
                }
                else if (selectedButton.name == "Return_MainMenu")
                {
                    Menu_Manager.Instance.OnGameMenu();
                }
                else if (selectedButton.name == "Tutorial")
                {
                    Menu_Manager.Tutorial_Mode = true;
                    Menu_Manager.Instance.In_Game();
                }
                else if (selectedButton.name == "Options")
                {
                    Menu_Manager.Instance.OnOptions();
                }
                else if (selectedButton.name == "Back")
                {
                    Menu_Manager.Instance.Resume();
                }
                else if (selectedButton.name == "Exit")
                {
                    Menu_Manager.Instance.Exit();
                }
                else if (selectedButton.name == "HighScore")
                {
                    Menu_Manager.Instance.HighScore();
                }
                else if (selectedButton.name == "Credits")
                {
                    Menu_Manager.Instance.Credits();
                }
                else if (selectedButton.name == "SFX_Check")
                {
                    ElapsedTime += Time.deltaTime;
                    if (ElapsedTime > EndTime)
                    {
                        if (selectedButton.GetComponentInParent<Toggle>().isOn == true)
                        {
                            selectedButton.GetComponentInParent<Toggle>().isOn = false;
                            if (TemptSFX == 0.0f)
                            {
                                TemptSFX = 0.5f;
                                Audio_Manager.Instance.SFXMaxAudio(TemptSFX);
                            }
                            else
                                Audio_Manager.Instance.SFXMaxAudio(TemptSFX);

                            ElapsedTime = 0;
                            Debug.Log("Off");
                        }
                        else
                        {
                            selectedButton.GetComponentInParent<Toggle>().isOn = true;
                            Audio_Manager.Instance.SFXMinAudio();
                            ElapsedTime = 0;
                            Debug.Log("On");
                        }
                    }
                    //selectedButton.OnDrag(eventData);
                }
                else if (selectedButton.name == "BGM_Check")
                {
                    ElapsedTime += Time.deltaTime;
                    if (ElapsedTime > EndTime)
                    {
                        if (selectedButton.GetComponentInParent<Toggle>().isOn == true)
                        {
                            selectedButton.GetComponentInParent<Toggle>().isOn = false;
                            if (TemptBGM == 0.0f)
                            {
                                TemptBGM = 0.5f;
                                Audio_Manager.Instance.BGMMaxAudio(TemptBGM);
                            }
                            else
                                Audio_Manager.Instance.BGMMaxAudio(TemptBGM);


                            ElapsedTime = 0;
                            Debug.Log("Off");
                        }
                        else
                        {
                            selectedButton.GetComponentInParent<Toggle>().isOn = true;
                            Audio_Manager.Instance.BGMMinAudio();
                            ElapsedTime = 0;
                            Debug.Log("On");
                        }
                    }
                }
                else if (selectedButton.name == "Increase_SFX" || selectedButton.name == "Increase_BGM")
                {
                    if (selectedButton.name == "Increase_BGM")
                    {
                        TemptBGM = selectedButton.GetComponentInParent<Slider>().value;
                        Audio_Manager.Instance.SetBgmLvl(selectedButton.GetComponentInParent<Slider>().value);
                    }
                    if (selectedButton.name == "Increase_SFX")
                    {
                        TemptSFX = selectedButton.GetComponentInParent<Slider>().value;
                        Audio_Manager.Instance.SetSfxLvl(selectedButton.GetComponentInParent<Slider>().value);
                    }

                    selectedButton.GetComponentInParent<Slider>().value += 40 * Time.deltaTime;
                    if (selectedButton.GetComponentInParent<Slider>().value >= 0)
                    {
                        selectedButton.GetComponentInParent<Slider>().value = 0;
                    }
                }
                else if (selectedButton.name == "Decrease_SFX" || selectedButton.name == "Decrease_BGM")
                {
                    if (selectedButton.name == "Decrease_BGM")
                    {
                        TemptBGM = selectedButton.GetComponentInParent<Slider>().value;
                        Audio_Manager.Instance.SetBgmLvl(selectedButton.GetComponentInParent<Slider>().value);
                    }
                    if (selectedButton.name == "Decrease_SFX")
                    {
                        TemptSFX = selectedButton.GetComponentInParent<Slider>().value;
                        Audio_Manager.Instance.SetSfxLvl(selectedButton.GetComponentInParent<Slider>().value);
                    }
                    selectedButton.GetComponentInParent<Slider>().value -= 40 * Time.deltaTime;
                    if (selectedButton.GetComponentInParent<Slider>().value <= -80)
                    {
                        selectedButton.GetComponentInParent<Slider>().value = -80;
                    }
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
