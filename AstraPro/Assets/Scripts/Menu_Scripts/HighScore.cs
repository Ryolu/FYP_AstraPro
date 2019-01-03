using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class HighScore : MonoBehaviour {

    public List<Text> highestScoreText;
    string path;
    string jsonString;
    public static HighScore Instance;
    public float overall;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Debug.Log("Testing");

        path = Application.dataPath + "/Scripts/Menu_Scripts/highScoreStorage.json";
        jsonString = File.ReadAllText(path);
    }

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
