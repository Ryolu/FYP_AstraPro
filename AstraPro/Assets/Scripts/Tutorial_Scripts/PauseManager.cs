using UnityEngine;

/// <summary>
/// This class is used for checking Pause status in the game.
/// This class is also used for Pause, Resume, Restart, Quit feature in the game.
/// 
/// Can be found attached in Canvas.
/// </summary>
public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    /// <summary>
    /// This is used for detecing and determining Game Pause state.
    /// 
    /// Default: False
    /// </summary>
    [HideInInspector]
    public bool isPaused = false;

    private void Awake()
    {
        // Set instance for other Scripts to access
        Instance = this;
    }

    /// <summary>
    /// This is used for pausing the game.
    /// Used in Pause Button in GameLevel and Tutorial Scenes.
    /// Can be found in Canvas -> "Pause".
    /// </summary>
    public void Pause()
    {
        isPaused = true;
    }

    /// <summary>
    /// This is used for resuming the game.
    /// Used in Resume Button in GameLevel and Tutorial Scenes.
    /// Can be found in Canvas -> PauseUI -> "Resume"
    /// </summary>
    public void Resume()
    {
        isPaused = false;
    }

    /// <summary>
    /// This is used for restarting the game.
    /// Used in Replay Button in GameLevel and Tutorial Scenes.
    /// Can be found in Canvas -> PauseUI -> "Replay"
    /// </summary>
    public void Replay()
    {
        Menu_Manager.Instance.In_Game();
    }

    /// <summary>
    /// This is used for exiting the game.
    /// Used in Quit Button in GameLevel and Tutorial Scenes.
    /// Can be found in Canvas -> PauseUI -> "Quit"
    /// </summary>
    public void ExitGame()
    {
        Menu_Manager.Instance.OnGameMenu();
    }
}