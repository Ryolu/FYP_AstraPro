using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RadialMenu : MonoBehaviour
{

    [SerializeField] List<IngredientSO> ingredientList;
    [SerializeField] List<Sprite> buttonTypes;
    [SerializeField] Button buttonPrefab;
    public FoodSO currentFood;
    public CookingAppliance currentAppliance;
    public RadialMenu otherMenu;

    private void Start()
    {
        //CallThisInsteadIngredient(7);
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
        if (transform.childCount == 1)
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

                Image foodPic = button.transform.GetChild(0).GetComponent<Image>();
                foodPic.sprite = ingredientList[i].sprite;
                foodPic.transform.rotation = Quaternion.identity * Quaternion.Euler(0, 90, 0);
                var ingredient = ingredientList[i];
                button.onClick.AddListener(() => currentAppliance.AddIngredient(ingredient));

                yield return new WaitForSeconds(0.075f);
            }
        }

        if (currentFood)
            ChangeColor(currentFood);
    }

    IEnumerator GenerateFoodButtons(CookingAppliance appliance)
    {
        int number = 3;
        otherMenu.currentAppliance = currentAppliance = appliance;

        for (int i = 0; i < number; i++)
        {
            float angle = (i * 360 / number + 90) * Mathf.Deg2Rad;
            float dist = -100;

            Button button = Instantiate(buttonPrefab, gameObject.transform);
            button.transform.Rotate(new Vector3(0, 0, angle * Mathf.Rad2Deg - 90));
            button.transform.localPosition += new Vector3(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist, 0);

            if (i < appliance.foodList.Count)
            {
                var food = appliance.foodList[i];
                if (otherMenu)
                    button.onClick.AddListener(() => otherMenu.currentFood = food);
                button.onClick.AddListener(() => appliance.ChooseFood(food));

                Image radialButton = button.GetComponent<Image>();
                radialButton.sprite = buttonTypes[1];
                radialButton.alphaHitTestMinimumThreshold = 0.5f;

                Image foodPic = button.transform.GetChild(0).GetComponent<Image>();
                foodPic.sprite = appliance.foodList[i].sprite;
                foodPic.transform.rotation = Quaternion.identity * Quaternion.Euler(0, 90, 0);
                foodPic.enabled = true;
                yield return new WaitForSeconds(0.075f);
            }
            else
            {
                Image radialButton = button.GetComponent<Image>();
                radialButton.sprite = buttonTypes[1];
                Color color = radialButton.color;
                color.a = 0.5f;
                radialButton.color = color;
                radialButton.alphaHitTestMinimumThreshold = 0.5f;
                button.interactable = false;

                Image foodPic = button.transform.GetChild(0).GetComponent<Image>();
                foodPic.enabled = false;
                yield return new WaitForSeconds(0.075f);
            }
        }
    }

    public void Close()
    {
        currentAppliance.NewFood();
        currentAppliance = null;
        currentFood = null;
    }

    public void ChangeColor(FoodSO foodSO)
    {
        List<Sprite> sprites = foodSO.ingredientList.Select(x => x.sprite).ToList();

        for (int i = 1; i < transform.childCount; i++)
        {
            if (sprites.Contains(transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite))
            {
                Button button = transform.GetChild(i).GetComponent<Button>();
                ColorBlock cb = button.colors;
                cb.highlightedColor = Color.green - Color.white * 0.3f;
                button.colors = cb;
            }
            else
            {
                Button button = transform.GetChild(i).GetComponent<Button>();
                ColorBlock cb = button.colors;
                cb.highlightedColor = Color.red - Color.white * 0.3f;
                button.colors = cb;
            }
        }
    }
}
