using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientPanel : MonoBehaviour {
    

    public void Zoomasaurus(bool isZoom)
    {
        if (isZoom)
            gameObject.transform.localScale *= 3;
        else
            gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
