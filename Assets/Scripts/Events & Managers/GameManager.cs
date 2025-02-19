using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;
    
    public static Action<GameStates> onGameStateChanged;
    public static Action TriggerMenuMusic;
    public static Action TriggerGameMusic;
    public static Action TriggerPauseMusic;
    public static Action TriggerResumeMusic;
    
    public static Action<bool> onToggleInput;
    
    public enum GameStates
    {
        MainMenu,
        GameLoop,
        GamePaused,
        GameResumed,
        GameOver,
    }

    private void Awake()
    {
        if (gameManagerInstance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            gameManagerInstance = this;
            DontDestroyOnLoad(gameObject);
            ChangeState(GameStates.MainMenu);
        }
    }

    public GameStates state;

    public void ChangeState(GameStates newState)
    {
        if(state == newState)
            return;
        
        state = newState;
        Debug.Log($"Game State Changed: {state}");
        
        onGameStateChanged?.Invoke(state);
        onToggleInput?.Invoke(state == GameStates.GameLoop);
        HandleStates(newState);

    }

    private void HandleStates(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.MainMenu:
                TriggerMenuMusic?.Invoke();
                Time.timeScale = 1;
                break;
            
            case GameStates.GameLoop:
                TriggerGameMusic?.Invoke();
                Time.timeScale = 1f;
                break;
            
            case GameStates.GamePaused:
                TriggerPauseMusic?.Invoke();
                Time.timeScale = 0f;
                break;
            
            case GameStates.GameResumed:
                TriggerResumeMusic?.Invoke();
                Time.timeScale = 1f;
                break;
            
            case GameStates.GameOver:
                TriggerPauseMusic?.Invoke();
                Time.timeScale = 0f;
                break;
        }
    }
}
