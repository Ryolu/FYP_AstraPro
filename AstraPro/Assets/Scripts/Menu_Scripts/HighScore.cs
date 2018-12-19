using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScore : MonoBehaviour {

    public Text[] highestScoreText;
    string path;
    string jsonString;

    void Start()
    {
        Debug.Log("Testing");

        path = Application.dataPath + "/Scripts/Menu_Scripts/highScoreStorage.json";
        jsonString = File.ReadAllText(path);

        ScoreStorage ScoreCheck = JsonUtility.FromJson<ScoreStorage>(jsonString);

        //highestScoreText[0].text = ScoreCheck.First.ToString();
        //highestScoreText[1].text = ScoreCheck.Second.ToString();
        //highestScoreText[2].text = ScoreCheck.Third.ToString();
        //highestScoreText[3].text = ScoreCheck.Fourth.ToString();
        //highestScoreText[4].text = ScoreCheck.Fifth.ToString();
        //highestScoreText[5].text = ScoreCheck.Sixth.ToString();
        //highestScoreText[6].text = ScoreCheck.Seventh.ToString();
        //highestScoreText[7].text = ScoreCheck.Eighth.ToString();
        //highestScoreText[8].text = ScoreCheck.Ninth.ToString();
        //highestScoreText[9].text = ScoreCheck.Tenth.ToString();
    }

    public void OnHighScore()
    {
    }

    [System.Serializable]
    public class ScoreStorage
    {
        public int First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth, Ninth, Tenth;
    }
}
