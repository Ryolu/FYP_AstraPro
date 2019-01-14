using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientButtonSpawn : MonoBehaviour {

    [SerializeField] List<IngredientSO> ingredientList;
    [SerializeField] Button buttonPrefab;
    public FoodSO currentFood;
    public CookingAppliance currentAppliance;

	// Use this for initialization
	void Start () {
        StartCoroutine(GenerateButtons(7));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator GenerateButtons(float number)
    {
        for (int i = 0; i < number; i++)
        {
            float angle = (i * 360 / number + 90) * Mathf.Deg2Rad;
            float dist = -200;
            Button button = Instantiate(buttonPrefab, gameObject.transform);
            button.transform.Rotate(new Vector3(0, 0, angle * Mathf.Rad2Deg - 90));
            button.transform.localPosition += new Vector3(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist, 0);
            //button.image.fillAmount = 1f / number;
            
            Image foodPic = button.transform.GetChild(0).GetComponent<Image>();
            foodPic.sprite = ingredientList[0].sprite;
            foodPic.transform.rotation = Quaternion.identity * Quaternion.Euler(0, 90, 0);

            yield return new WaitForSeconds(0.075f);
        }
    }
}
