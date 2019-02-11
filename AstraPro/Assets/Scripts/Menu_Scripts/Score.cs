using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public static Score Instance;    
    public float rate = .20f; //set the rate 
    public Text money; //for setting the text
    public GameObject fillimage; //setting a image for the fill bar
    public Image rateBar; //for the back ground of the bar
    public List<Image> Flower = new List<Image>(); //for the rate flower to be set in
    float elapsedTime; // for timer to run
    float winGameTimerCount; //for win timer count down
    float endGameTime = 60.0f; //if after 60 of the game reached max rate go to win
    float endTime = 5.0f; //timer set to fade out the image behind
    bool switching = false; //switching on and off the fade
    public bool maxStar = false; //if you reached all 5 stars

    //for the start of the score being called
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
	
	// Update is called once per frame
	void Update ()
    {
        //if game not paused then go back to all on
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

    //if rate up will have a flower will appeared
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

    //profit counter for the game
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
