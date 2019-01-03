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
        SceneManager.LoadScene("Scenes/Main_Menu");
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
        Audio_Manager.Instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadSceneAsync("Scenes/HighScore");
    }

    public void Credits()
    {
        Audio_Manager.Instance.GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["Score_Board"];
        Audio_Manager.Instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Scenes/Credits");
    }

    public void GameOver()
    {
        //Audio_Manager.Instance.GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["Score_Board"];
        //Audio_Manager.Instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Scenes/GameOver");
    }

    public void In_Game()
    {
        if (Tutorial_Mode == true)
        {
            Audio_Manager.Instance.GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["Tutorial"];
            Audio_Manager.Instance.GetComponent<AudioSource>().Play();
            SceneManager.LoadSceneAsync("Scenes/Tutorial/Tutorial");
            Tutorial_Mode = false;
        }
        else
        {
            Audio_Manager.Instance.GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["RunningOutOfTime_BGM"];
            Audio_Manager.Instance.GetComponent<AudioSource>().Play();
            SceneManager.LoadSceneAsync("Scenes/GameLevel");
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
