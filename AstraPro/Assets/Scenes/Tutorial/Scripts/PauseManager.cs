using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;

    public static void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }

    public static void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }
}
