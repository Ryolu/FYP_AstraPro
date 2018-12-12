using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class Menu_Manager : MonoBehaviour {
    static public bool Tutorial_Mode = false;
    public GameObject usingAudio;
    public static Menu_Manager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    public void OnGameMenu()
    {
        Audio_Manager.Instance.transform.GetChild(0).GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["MainMenu"];
        SceneManager.LoadSceneAsync("Scenes/Main_Menu");
    }

    public void OnOptions()
    {
        SceneManager.LoadSceneAsync("Scenes/Options", LoadSceneMode.Additive);
    }

    public void Resume()
    {
        SceneManager.UnloadSceneAsync("Scenes/Options");
    }

    public void HighScore()
    {
        Audio_Manager.Instance.transform.GetChild(0).GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["Score_Board"];
        SceneManager.LoadSceneAsync("Scenes/HighScore");
    }

    public void Credits()
    {
        SceneManager.LoadSceneAsync("Scenes/Credits");
    }

    public void In_Game()
    {
        if (Tutorial_Mode == true)
        {
            SceneManager.LoadSceneAsync("Scenes/Tutorial/Tutorial");
        }
        else
        {
            SceneManager.LoadSceneAsync("Scenes/GameLevel");
        }
    }
}
