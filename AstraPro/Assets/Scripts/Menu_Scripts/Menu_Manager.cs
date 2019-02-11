using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class Menu_Manager : MonoBehaviour {

    //all game objects
    public static Menu_Manager Instance;
    public GameObject mainCanvas;
    public GameObject guideBookCanvas;
    public GameObject usingAudio;
    public bool Tutorial_Mode = false;

    //Do not destroy this cause its link to all scenes
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    //Set to tutorial mode or not
    public void SetTutorial_Mode(bool mode)
    {
        Tutorial_Mode = mode;
    }

    //Go to main menu
    public void OnGameMenu()
    {
        Audio_Manager.Instance.GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["MainMenu"];
        Audio_Manager.Instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Scenes/Main_Menu");
    }

    //Go to Options
    public void OnOptions()
    {
        Audio_Manager.Instance.GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["Score_Board"];
        Audio_Manager.Instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Scenes/Options");
    }

    //Resume to the game
    public void Resume()
    {
        SceneManager.UnloadSceneAsync("Scenes/Options");
    }

    //Go to highscore
    public void HighScore()
    {
        Audio_Manager.Instance.GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["Score_Board"];
        Audio_Manager.Instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadSceneAsync("Scenes/HighScore");
    }

    //Go to Credits
    public void Credits()
    {
        Audio_Manager.Instance.GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["Score_Board"];
        Audio_Manager.Instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Scenes/Credits");
    }

    //Go to GameOver
    public void GameOver()
    {
        Audio_Manager.Instance.GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["Score_Board"];
        Audio_Manager.Instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Scenes/GameOver");
    }

    //Go to WinGame 
    public void WinGame()
    {
        Audio_Manager.Instance.GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["Score_Board"];
        Audio_Manager.Instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Scenes/WinGame");
    }

    //Go to open the guide book
    public void OpenGuideBook()
    {
        guideBookCanvas.SetActive(true);
    }

    //close the game guide book
    public void CloseGuideBook()
    {
        guideBookCanvas.SetActive(false);
    }

    //go choose game mode
    public void In_Game()
    {
        //go to tutorial true go to tutorial mode
        if (Tutorial_Mode == true)
        {
            Audio_Manager.Instance.GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["Tutorial"];
            Audio_Manager.Instance.GetComponent<AudioSource>().Play();
            SceneManager.LoadSceneAsync("Scenes/Tutorial/Tutorial");
        }
        //go into main game
        else
        {
            Audio_Manager.Instance.GetComponent<AudioSource>().clip = Audio_Manager.Instance.audioDictionary["RunningOutOfTime_BGM"];
            Audio_Manager.Instance.GetComponent<AudioSource>().Play();
            SceneManager.LoadSceneAsync("Scenes/GameLevel");
        }
    }

    //Turn off applications
    public void Exit()
    {
        Application.Quit();
    }
}
