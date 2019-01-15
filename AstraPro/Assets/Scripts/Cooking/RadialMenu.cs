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

    private void Start()
    {
        CallThisInstead(7);
    }

    public void CallThisInstead(int number)
    {
        StartCoroutine(GenerateButtons(number));
    }

    IEnumerator GenerateButtons(int number)
    {
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
            radialButton.alphaHitTestMinimumThreshold = 0.9f;
            //button.image.fillAmount = 1f / number;
            
            Image foodPic = button.transform.GetChild(0).GetComponent<Image>();
            foodPic.sprite = ingredientList[i].sprite;
            foodPic.transform.rotation = Quaternion.identity * Quaternion.Euler(0, 90, 0);

            yield return new WaitForSeconds(0.075f);
        }
    }
}
