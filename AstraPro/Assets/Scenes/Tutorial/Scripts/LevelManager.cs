using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public List<CookingAppliance> cookingAppliances;

	void Awake ()
    {
        Instance = this;
	}
}
