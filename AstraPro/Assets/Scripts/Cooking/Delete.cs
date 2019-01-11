using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Delete : MonoBehaviour {

    public List<IngredientSO> ingredientList;
    public Button buttonPrefab;
    Image foodPic;

	// Use this for initialization
	void Start () {
        StartCoroutine(GenerateButtons());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator GenerateButtons()
    {
        float number = 100;

        for (int i = 0; i < number; i++)
        {
            float angle = (i * 360 / number + 90) * Mathf.Deg2Rad;
            float dist = 50;
            Button button = Instantiate(buttonPrefab, gameObject.transform.position + new Vector3( Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist, 0), Quaternion.identity, gameObject.transform);
            button.transform.Rotate(new Vector3(0, 0, 360f / (number * 2) + (360f / number * i)));
            button.image.fillAmount = 1f / number;
            button.image.alphaHitTestMinimumThreshold = 0.9f;
            Debug.Log("balls");
            
            foodPic = button.transform.GetChild(0).GetComponent<Image>();
            foodPic.sprite = ingredientList[0].sprite;
            foodPic.transform.rotation = Quaternion.identity;

            yield return new WaitForSeconds(0.075f);
        }
    }
}
