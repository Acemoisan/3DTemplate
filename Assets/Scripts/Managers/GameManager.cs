using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}


[System.Serializable]
public enum GameMode{
    Survival,
    Creative
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameState _currentGameState = GameState.MainMenu;
    public GameState CurrentGameState { get { return _currentGameState; } }
    [SerializeField] private GameMode _currentGameMode = GameMode.Survival;
    public GameMode CurrentGameMode {get { return _currentGameMode; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        ChangeGameState(GameState.Playing);
    }

    public void ReturnToMainMenu()
    {
        ChangeGameState(GameState.MainMenu);
    }

    private void ChangeGameState(GameState newState)
    {
        _currentGameState = newState;
        // Handle any general game state changes, e.g., updating UI
    }

    public void ChangeGameMode(GameMode newGameMode)
    {
        _currentGameMode = newGameMode;
        // Handle any general game mode changes, e.g., updating UI
    }
}
