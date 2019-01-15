using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour {

    [SerializeField] List<IngredientSO> ingredientList;
    [SerializeField] List<Sprite> buttonTypes;
    [SerializeField] Button buttonPrefab;
    public FoodSO currentFood;
    public CookingAppliance currentAppliance;
    public RadialMenu otherMenu;

    private void Start()
    {
        //CallThisInstead(7);
    }

    public void CallThisInsteadIngredient(int number)
    {
        StartCoroutine(GenerateIngredientButtons(number));
    }

    public void CallThisInsteadFood(CookingAppliance appliance)
    {
        StartCoroutine(GenerateFoodButtons(appliance));
    }

    IEnumerator GenerateIngredientButtons(int number)
    {
        //Debug.Log("Generating ingredient buttons");
        for (int i = 0; i < number; i++)
        {
            float angle = (i * 360 / number + 90) * Mathf.Deg2Rad;
            Button button = Instantiate(buttonPrefab, gameObject.transform);
            button.transform.Rotate(new Vector3(0, 0, angle * Mathf.Rad2Deg - 90));
            Image radialButton = button.GetComponent<Image>();
            float dist = -200;
            switch (number)
            {
                case 1:
                    radialButton.sprite = buttonTypes[0];
                    dist = 0;
                    break;
                case 3:
                    radialButton.sprite = buttonTypes[1];
                    break;
                case 7:
                    radialButton.sprite = buttonTypes[2];
                    break;
                default:
                    break;
            }
            button.transform.localPosition += new Vector3(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist, 0);
            radialButton.alphaHitTestMinimumThreshold = 0.5f;
            //button.image.fillAmount = 1f / number;
            
            Image foodPic = button.transform.GetChild(0).GetComponent<Image>();
            foodPic.sprite = ingredientList[i].sprite;
            foodPic.transform.rotation = Quaternion.identity * Quaternion.Euler(0, 90, 0);
            var ingredient = ingredientList[i];
            button.onClick.AddListener(() => currentAppliance.AddIngredient(ingredient));

            yield return new WaitForSeconds(0.075f);
        }
    }

    IEnumerator GenerateFoodButtons(CookingAppliance appliance)
    {
        int number = appliance.foodList.Count;
        otherMenu.currentAppliance = currentAppliance = appliance;
        //string list = "";
        //foreach (FoodSO foodso in appliance.foodList)
        //{
        //    list += foodso.foodName + ", ";
        //}
        //Debug.Log(list);

        for (int i = 0; i < number; i++)
        {
            float angle = (i * 360 / number + 90) * Mathf.Deg2Rad;
            float dist = -200;

            Button button = Instantiate(buttonPrefab, gameObject.transform);
            button.transform.Rotate(new Vector3(0, 0, angle * Mathf.Rad2Deg - 90));
            button.transform.localPosition += new Vector3(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist, 0);
            Debug.Log(appliance.foodList[i]);
            var food = appliance.foodList[i];
            //button.onClick.AddListener(() => Test(appliance.foodList[0]));
            button.onClick.AddListener(() => appliance.ChooseFood(food));

            Image radialButton = button.GetComponent<Image>();
            radialButton.sprite = buttonTypes[1];
            radialButton.alphaHitTestMinimumThreshold = 0.5f;

            Image foodPic = button.transform.GetChild(0).GetComponent<Image>();
            foodPic.sprite = appliance.foodList[i].sprite;
            foodPic.transform.rotation = Quaternion.identity * Quaternion.Euler(0, 90, 0);
            yield return new WaitForSeconds(0.075f);
        }
    }

    void Test(FoodSO food)
    {
        currentFood = food;
    }
}
