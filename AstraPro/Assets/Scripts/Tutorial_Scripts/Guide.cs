using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// This class manages Guide Feature in Tutorial Scene.
/// 
/// Can be found attached in Canvas -> GuideImage
/// </summary>
public class Guide : MonoBehaviour
{
    public static Guide Instance;

    /// <summary>
    /// Guide sprites.
    /// </summary>
    [Tooltip("Guide sprites.\n" +
        "\n" +
        "Can be found in Assets -> UI -> Guide" +
        "Please sort in ascending numeric order.")]
    [SerializeField] private List<Sprite> sprites;

    /// <summary>
    /// A float that determines the time to switch guide sprite.
    /// 
    /// Default: 3
    /// </summary>
    [Tooltip("A float that determines the time to switch to the next guide sprite.\n" +
        "Default: 3")]
    [SerializeField] private float endTime = 3f;

    /// <summary>
    /// Food List Panel(A menu used to select the food to cook).
    /// </summary>
    [Tooltip("Food List Panel.\n" +
        "Can be found in Canvas")]
    [SerializeField] private GameObject foodListPanel;

    /// <summary>
    /// Booleans to detemine current status of Guide.
    /// To find out what to show next.
    /// 
    /// Default: False
    /// </summary>
    [HideInInspector] public bool finishedIntro = false;
    [HideInInspector] public bool finishedOrder = false;
    [HideInInspector] public bool finishedCookNServe = false;
    [HideInInspector] public bool finishedGuide = false;

    /// <summary>
    /// The Image Component of this GameObject
    /// </summary>
    private Image thisImage;

    /// <summary>
    /// A float timer used to switch to next guide sprite by endTime above
    /// </summary>
    private float elapsedTime;

    /// <summary>
    /// A boolean used to determine to hide or show the Guide Image
    /// 
    /// Default: False
    /// </summary>
    private bool hidden = false;

    /// <summary>
    /// An integer used to track current Guide Sprite
    /// 
    /// Default: 0
    /// </summary>
    private int index = 0;

    private void Awake()
    {
        // Set instance for other Scripts to access
        Instance = this;
    }

    private void Start ()
    {
        //Initialise Image and Sprite
        thisImage = GetComponent<Image>();
        thisImage.sprite = sprites[index];
	}
	
    /// <summary>
    /// Activates this GameObject.
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
        thisImage.color = new Color(thisImage.color.r, thisImage.color.g, thisImage.color.b, 1f);
        hidden = false;
    }

    /// <summary>
    /// Deactivates this GameObject and Resume the Game.
    /// </summary>
    private void Hide()
    {
        gameObject.SetActive(false);
        thisImage.color = new Color(thisImage.color.r, thisImage.color.g, thisImage.color.b, 0f);
        hidden = true;
        PauseManager.Instance.Resume();
    }

    /// <summary>
    /// Pauses the game and Show the Guide Image as necessary while counting the timer for switching Guide Sprites.
    /// </summary>
    private void TimeUpdateImage()
    {
        // If the game is not paused, Pause it
        if (!PauseManager.Instance.isPaused)
        {
            PauseManager.Instance.Pause();
        }

        // Runs timer
        elapsedTime += Time.deltaTime;

        // Activates the Guide Image if its not
        if (hidden)
        {
            Show();
        }

        // When timer reach limit
        if (elapsedTime >= endTime)
        {
            UpdateImage();
        }
    }

    /// <summary>
    /// Resets Timer, Switch to next Guide Sprite.
    /// </summary>
    public void UpdateImage()
    {
        // Reset Timer
        elapsedTime = 0f;

        // Switch to next Guide Sprite
        index++;

        // Prevent Out of Array by setting to last Guide Sprite
        if (index < sprites.Count)
        {
            thisImage.sprite = sprites[index];
        }
    }

    /// <summary>
    /// Check if already finish guiding about Cooking
    /// </summary>
    /// <returns> A boolean that shows Finished guiding about Cooking or not. </returns>
    public bool CheckIfGuidedCook()
    {
        return (index > 17) ? true : false;
    }

    private void Update()
    {
        // If finished guiding,do not do anything
        if (finishedGuide)
        {
            return;
        }
        
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

                // Hides the Guide Image and Resume the game for next section
                if (!hidden)
                {
                    Hide();
                }
            }
        }
        else if(finishedIntro && !finishedOrder && !finishedCookNServe)
        {
            // When all active customers have reached their movement target
            if (CustomerSpawner.Instance.customerDic.All(x => x.Value.reachedTarget == true))
            {
                // 4,5,6,7,8,9 is about customer order (Order)
                if (index > 3 && index <= 9)
                {
                    TimeUpdateImage();
                }
                else
                {
                    finishedOrder = true;

                    // Hides the Guide Image and Resume the game for next section
                    if (!hidden)
                    {
                        Hide();
                    }
                }
            }
        }
        else if (finishedIntro && finishedOrder && !finishedCookNServe)
        {
            // When Food List Panel is active
            if (foodListPanel.activeSelf)
            {
                // 10,11,12,13,14,15,16,17,18,19 is about Cooking & Serving food (CookNServe)
                if (index > 9 && index <= 19)
                {
                    TimeUpdateImage();
                }
                else
                {
                    finishedCookNServe = true;

                    // Hides the Guide Image and Resume the game for next section
                    if (!hidden)
                    {
                        Hide();
                    }
                }
            }
        }
        else if (finishedCookNServe)
        {
            // When any of the customer is fighting with player
            if (CustomerSpawner.Instance.customerDic.Any(x => x.Value.fighting == true))
            {
                if (index > 19 && index <= sprites.Count)
                {
                    TimeUpdateImage();
                }
                else
                {
                    finishedGuide = true;

                    // Hides the Guide Image and Resume the game
                    if (!hidden)
                    {
                        Hide();
                    }
                }
            }
        }
    }
}
