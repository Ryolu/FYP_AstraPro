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
    public Ingredient[] ingredientList;
    public bool[] cookingSteps;
    public Sprite sprite;

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
