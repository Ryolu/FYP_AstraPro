using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public static Score Instance;    
    public float rate = .20f;
    public Text money;
    public Image rateBar;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        HighScore.Instance.overall = 100.0f;
        rate = .20f;
	}
	
	// Update is called once per frame
	void Update () {
       
        if (Input.GetKeyDown("space"))
        {
            HighScore.Instance.overall += 100;
            rate = 0;
        }
        if (rate <= 0)
        {
            Menu_Manager.Instance.GameOver();
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

        rateBar.fillAmount = rate;
        money.text = "Profit " + HighScore.Instance.overall.ToString();
        HighScore.Instance.overall += food.cost;
    }
}
