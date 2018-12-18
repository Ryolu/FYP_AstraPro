using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// A ScriptableObject for food containing
/// its data
/// </summary>
public class FoodSO : ScriptableObject {

    public string foodName = "Default";
    /// <summary>
    /// List of ingredients
    /// </summary>
    public List<IngredientSO> ingredientList;
    /// <summary>
    /// Time taken to cook this food
    /// </summary>
    public int timer;
    /// <summary>
    /// Time taken to clean up for the next food to be cooked
    /// </summary>
    public int cleanTimer;
    /// <summary>
    /// The food's sprite
    /// </summary>
    public Sprite sprite;
    /// <summary>
    /// The prefab containing the model and the other necessary data for this food
    /// </summary>
    public GameObject prefab;

#if UNITY_EDITOR

    /// <summary>
    /// Creates the food asset
    /// </summary>
    [MenuItem("Assets/Create/Custom/Food")]
    public static void CreateFood()
    {
        FoodSO asset = CreateInstance<FoodSO>();

        AssetDatabase.CreateAsset(asset, "Assets/Foods/food.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
#endif

}
