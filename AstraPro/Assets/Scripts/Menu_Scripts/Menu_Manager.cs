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
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    public void OnGameMenu()
    {
        Audio_Manager.Instance.GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["MainMenu"];
        Audio_Manager.Instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadSceneAsync("Scenes/Main_Menu");
    }

    public void OnOptions()
    {
        Audio_Manager.Instance.GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["Score_Board"];
        Audio_Manager.Instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Scenes/Options");
    }

    public void Resume()
    {
        SceneManager.UnloadSceneAsync("Scenes/Options");
    }

    public void HighScore()
    {
        Audio_Manager.Instance.GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["Score_Board"];
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

    public void Exit()
    {
        Application.Quit();
    }
}
