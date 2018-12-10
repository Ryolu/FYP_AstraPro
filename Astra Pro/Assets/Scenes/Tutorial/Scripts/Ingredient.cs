using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Ingredient : ScriptableObject {

    public string ingredientName = "Default";
    public float cost = 0;
    public Sprite sprite;

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Custom/Ingredient")]
    public static void CreateIngredient()
    {
        Ingredient asset = CreateInstance<Ingredient>();

        AssetDatabase.CreateAsset(asset, "Assets/Ingredients/ingredient.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
#endif
}
