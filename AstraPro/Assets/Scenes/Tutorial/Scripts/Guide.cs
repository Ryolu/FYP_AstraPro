using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

public class Guide : MonoBehaviour
{
    public static Guide Instance;
    [Tooltip("Guide images. Please sort in ascending numeric order")] [SerializeField] private List<Sprite> images;
    [Tooltip("Time for one guide image to switch")] [SerializeField] private float endTime = 3f;
    [Tooltip("Food List Panel in Canvas")] [SerializeField] private GameObject foodListPanel;

    private Image thisImage;
    private float elapsedTime;
    [HideInInspector] public bool finishedIntro = false;
    [HideInInspector] public bool finishedOrder = false;
    [HideInInspector] public bool finishedCookNServe = false;
    [HideInInspector] public bool finishedGuide = false;
    private bool hidden = false;
    private int index = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start ()
    {
        thisImage = GetComponent<Image>();
        thisImage.sprite = images[index];
	}
	
    public void Show()
    {
        gameObject.SetActive(true);
        thisImage.color = new Color(thisImage.color.r, thisImage.color.g, thisImage.color.b, 1f);
        hidden = false;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        thisImage.color = new Color(thisImage.color.r, thisImage.color.g, thisImage.color.b, 0f);
        hidden = true;
        PauseManager.Instance.Resume();
    }

    private void TimeUpdateImage()
    {
        PauseManager.Instance.Pause();

        elapsedTime += Time.deltaTime;

        if (hidden)
        {
            Show();
        }

        if (elapsedTime >= endTime)
        {
            UpdateImage();
        }
    }

    public void UpdateImage()
    {
        elapsedTime = 0f;
        
        index++;

        if (index < images.Count)
        {
            thisImage.sprite = images[index];
        }
    }

    public bool CheckIfGuidedCook()
    {
        return (index > 17) ? true : false;
    }

    private void Update()
    {
        if (finishedGuide) return;

        //Debug.Log(index); 
        if (!finishedIntro && !finishedOrder && !finishedCookNServe)
        {
            // 0,1,2,3 is about satisfying bar and game objective (Intro)
            if (index <= 3)
            {
                TimeUpdateImage();
            }
            else
            {
                finishedIntro = true;
                if (!hidden)
                {
                    Hide();
                }
            }
        }
        else if(finishedIntro && !finishedOrder && !finishedCookNServe)
        {
            if (CustomerSpawner.Instance.customerDic.All(x => x.Value.reachedTarget == true))
            {
                // 4,5,6,7 is about customer order (Order)
                if (index > 3 && index <= 7)
                {
                    TimeUpdateImage();
                }
                else
                {
                    finishedOrder = true;
                    if (!hidden)
                    {
                        Hide();
                    }
                }
            }
        }
        else if (finishedIntro && finishedOrder && !finishedCookNServe)
        {
            if (foodListPanel.activeSelf)
            {
                // 8,9,10,11,12,13,14,15,16,17 is about Cooking & Serving food (CookNServe)
                if (index > 7 && index <= 17)
                {
                    TimeUpdateImage();
                }
                else
                {
                    finishedCookNServe = true;
                    if (!hidden)
                    {
                        Hide();
                    }
                }
            }
        }
        else if (finishedCookNServe)
        {
            if (CustomerSpawner.Instance.customerDic.Any(x => x.Value.fighting == true))
            {
                if (index > 17 && index <= images.Count)
                {
                    TimeUpdateImage();
                }
                else
                {
                    finishedGuide = true;
                    if (!hidden)
                    {
                        Hide();
                    }
                }
            }
        }
    }
}
