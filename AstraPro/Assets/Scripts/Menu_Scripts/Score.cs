using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public static Score Instance;    
    public float rate = .20f;
    public Text money;
    public Image fillimage;
    public Image rateBar;
    public List<Image> Flower = new List<Image>();
    float elapsedTime;
    float endTime2 = 0.5f;
    float endTime = 5.0f;
    bool switching = false;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        HighScore.Instance.overall = 100.0f;
        switching = true;
        rate = .40f;
	}
	
	// Update is called once per frame
	void Update () {
        if (switching)
        {
            elapsedTime += Time.deltaTime;
            fillimage.gameObject.SetActive(true);
            fillimage.gameObject.GetComponent<Animation>().Play();
            if (elapsedTime > endTime)
            {
                fillimage.gameObject.SetActive(false);
                switching = false;
                elapsedTime = 0;
            }
        }
        //if (Input.GetKeyDown("space"))
        //{
        //    HighScore.Instance.overall += 100;
        //    rate = 0;
        //}
        if (rate <= 0)
        {
            Menu_Manager.Instance.GameOver();
        }
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

    public void Profit(FoodSO food)
    {
        if (food.foodName == "Curry Fish Head")
            rate += 0.1f;
        else if (food.foodName == "Laksa")
            rate += 0.03f;
        else if (food.foodName == "Mee Siam")
            rate += 0.01f;
        else if (food.foodName == "Otah")
            rate += 0.05f;

        switching = true;
        Total_Rate();
        rateBar.fillAmount = rate;
        money.text = HighScore.Instance.overall.ToString();
        HighScore.Instance.overall += food.cost;
    }
}
