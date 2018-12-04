using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_Manager : MonoBehaviour {

    static public void OnGameMenu()
    {
        Debug.Log("TEST");
        SceneManager.LoadScene("Scenes/TestScene3");
    }

    static public void OnReturn()
    {

    }

    
}
