using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class manages a list of Cooking Appliances in the GameLevel and Tutorial Scenes.
/// 
/// Can be found attached in LevelManager.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Tooltip("The list of Cooking Appliances which can be use to cook food in the game.\n" +
        "\n" +
        "Can be found in Environment Prefab -> Interactable_objects -> Cookwares.\n" +
        "Named as 'Frying_pan', 'Pot_1', 'Pot_2'.")]
    /// <summary>
    /// The list of Cooking Appliances which can be use to cook food in the game.
    /// </summary>
    public List<CookingAppliance> cookingAppliances;

	private void Awake ()
    {
        // Set instance for other Scripts to access
        Instance = this;
	}
}