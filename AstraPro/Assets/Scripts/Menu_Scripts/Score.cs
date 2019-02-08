using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public static Score Instance;    
    public float rate = .20f;
    public Text money;
    public GameObject fillimage;
    public Image rateBar;
    public List<Image> Flower = new List<Image>();
    float elapsedTime;
    float winGameTimerCount;
    float endGameTime = 60.0f;
    float endTime = 5.0f;
    bool switching = false;
    public bool maxStar = false;

    private void Awake()
    {
        Instance = this;

        HighScore.Instance.overall = 100.0f;
        switching = true;
        maxStar = false;
        rate = .40f;

        Total_Rate();
        rateBar.fillAmount = rate;
        money.text = HighScore.Instance.overall.ToString();
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.isPaused) return;

        if (switching)
        {
            elapsedTime += Time.deltaTime;
            //fillimage.gameObject.SetActive(true);
            fillimage.gameObject.GetComponent<Animation>().Play();
            if (elapsedTime > endTime)
            {
                //fillimage.gameObject.SetActive(false);
                switching = false;
                elapsedTime = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rate = 1.0f;
        }
        if (rate >= 1.0f)
        {
            maxStar = true;
        }
        else if (rate < 1.0f)
        {
            maxStar = false;
        }

        if (rate <= 0)
        {
            Menu_Manager.Instance.GameOver();
        }
        if (rate >= 1.0f)
        {
            Menu_Manager.Instance.WinGame();
        }

        //Debug.Log(winGameTimerCount);
    }

    public void Total_Rate()
    {
        int starCount = (int)(rate * 5);
        foreach(Image go in Flower)
        {
            go.gameObject.SetActive(false);
        }
        for(int i = 0; i < starCount; ++i)
        {
            Flower[i].gameObject.SetActive(true);
        }
    }

    public void Profit(FoodSO food, float time)
    {
        if (food.foodName == "Curry Fish Head")
            rate += (0.2f * time);
        else if (food.foodName == "Laksa")
            rate += (0.05f * time);
        else if (food.foodName == "Mee Siam")
            rate += (0.05f * time);
        else if (food.foodName == "Otah")
            rate += (0.1f * time);

        switching = true;
        rateBar.fillAmount = rate;
        Total_Rate();
        HighScore.Instance.overall += food.cost;
        money.text = HighScore.Instance.overall.ToString();
    }
}
