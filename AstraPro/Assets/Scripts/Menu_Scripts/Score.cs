using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Score : MonoBehaviour {
    public static Score instance;
    public float Overall = 100.0f;
    public float Otah = 5.0f;
    public float CurryFishHead = 10.0f;
    public float Laksa = 6.0f;
    public float MeeSiam = 5.5f; 
    public float Rate = .20f;
    public Text Money;
    public Image RateBar;
    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        Overall = 100.0f;
        Rate = .20f;
	}
	
	// Update is called once per frame
	void Update () {
        Money.text = "Profit " + Overall.ToString();
        RateBar.fillAmount = Rate;
	}

    public void Profit(FoodSO food)
    {
        if (food.foodName == "Curry Fish Head")
        {
            Overall += CurryFishHead;
            Rate += 0.1f;
        }
        else if (food.foodName == "Laksa")
        {
            Overall += Laksa;
            Rate += 0.03f;
        }
        else if (food.foodName == "Mee Siam")
        {
            Overall += MeeSiam;
            Rate += 0.01f;
        }
        else if (food.foodName == "Otah")
        {
            Overall += Otah;
            Rate += 0.05f;
        }
    }
}
