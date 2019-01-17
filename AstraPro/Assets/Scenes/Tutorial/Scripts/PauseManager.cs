using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public bool isPaused = false;

    private void Awake()
    {
        Instance = this;
    }

    public void Pause()
    {
        isPaused = true;
        //Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        //Time.timeScale = 1f;
    }
}
