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
        GameRestarted,
        GameOver,
    }

    public GameStates state;
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

    public void ChangeState(GameStates newState)
    {
        if(state == newState)
            return;

        state = newState;
        onGameStateChanged?.Invoke(state);
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
                PlayerPrefs.SetInt("currentScore", 0);
                TriggerGameMusic?.Invoke();
                onToggleInput?.Invoke(true);
                Time.timeScale = 1f;
                break;
            
            case GameStates.GamePaused:
                TriggerPauseMusic?.Invoke();
                Time.timeScale = 0f;
                onToggleInput?.Invoke(false);
                break;
            
            case GameStates.GameResumed:
                TriggerResumeMusic?.Invoke();
                Time.timeScale = 1f;
                onToggleInput?.Invoke(true);
                break;
            
            case GameStates.GameRestarted:
                TriggerGameMusic?.Invoke();
                Time.timeScale = 1f;
                onToggleInput?.Invoke(true);
                break;
            
            case GameStates.GameOver:
                TriggerPauseMusic?.Invoke();
                onToggleInput?.Invoke(false);
                Time.timeScale = 0f;
                break;
        }
    }
    
    public void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("SFXVolume");
        PlayerPrefs.DeleteKey("ButtonsVolume");
        PlayerPrefs.DeleteKey("MusicVolume");
        PlayerPrefs.DeleteKey("currentHealth");
        PlayerPrefs.DeleteKey("currentScore");
        
        Application.Quit();
    }
}
