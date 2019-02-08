using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class HighScore : MonoBehaviour {
    //taking file from json file and read all the text to place into the game
    public List<Text> highestScoreText;
    string path;
    public string fileName;
    string jsonString;
    public static HighScore Instance;
    public float overall;

    //going to highscore and do not destory the game object
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        fileName = "highScoreStorage.json";
    }

    //find the game file and read the file from scripts
    void Start()
    {
        if (Debug.isDebugBuild)
            path = Application.dataPath + "/Scripts/Menu_Scripts/" + fileName;
        else
            path = Application.dataPath + "/" + fileName;
        jsonString = File.ReadAllText(path);
    }

    //Score change if win or lose
    public void OnHighScore()
    {
        ScoreStorage ScoreCheck = JsonUtility.FromJson<ScoreStorage>(jsonString);
        List<float> tempList = new List<float>();
        foreach (int score in ScoreCheck.scoreList)
            tempList.Add(score);

        tempList.Add(overall);
        tempList = tempList.OrderByDescending(x => x).ToList();
        for (int i = 0; i < highestScoreText.Count; i++)
            highestScoreText[i].text = tempList[i].ToString();
    }

    [System.Serializable]
    public class ScoreStorage
    {
        public List<int> scoreList;
    }
}
