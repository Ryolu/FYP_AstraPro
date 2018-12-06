using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_Manager : MonoBehaviour {

    static public void OnGameMenu()
    {
        SceneManager.LoadScene("Scenes/Main_Menu");
    }

    static public void OnOptions()
    {
        SceneManager.LoadScene("Scenes/Options", LoadSceneMode.Additive);
    }

    static public void Resume()
    {
        SceneManager.UnloadSceneAsync("Scenes/Options");
    }

    static public void HighScore()
    {
        SceneManager.LoadScene("Scenes/HighScore");
    }

    static public void Credits()
    {
        SceneManager.LoadScene("Scenes/Credits");
    }

    static public void In_Game()
    {
        SceneManager.LoadScene("Scenes/GameLevel");
    }
}
