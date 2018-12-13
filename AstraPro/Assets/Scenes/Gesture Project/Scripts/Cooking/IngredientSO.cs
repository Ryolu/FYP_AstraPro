using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// A ScriptableObject class for storing ingredient information
/// </summary>
public class IngredientSO : ScriptableObject {

    public string ingredientName = "Default";
    public float cost = 0;
    public Sprite sprite;
    public GameObject go;

#if UNITY_EDITOR
    /// <summary>
    /// Creates the ingredient from Unity Editor
    /// </summary>
    [MenuItem("Assets/Create/Custom/Ingredient")]
    public static void CreateIngredient()
    {
        IngredientSO asset = CreateInstance<IngredientSO>();

        AssetDatabase.CreateAsset(asset, "Assets/Ingredients/ingredient.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
#endif
}
