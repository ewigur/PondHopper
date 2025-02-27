using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GMInstance;
    
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
        if (GMInstance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            GMInstance = this;
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
                onToggleInput?.Invoke(true);
                PlayerPrefs.SetInt("currentScore", 0);
                TriggerGameMusic?.Invoke();
                Time.timeScale = 1f;
                break;
            
            case GameStates.GamePaused:
                onToggleInput?.Invoke(false);
                TriggerPauseMusic?.Invoke();
                Time.timeScale = 0f;
                break;
            
            case GameStates.GameResumed:
                onToggleInput?.Invoke(true);
                TriggerResumeMusic?.Invoke();
                Time.timeScale = 1f;
                break;
            
            case GameStates.GameRestarted:
                onToggleInput?.Invoke(true);
                TriggerResumeMusic?.Invoke();
                Time.timeScale = 1f;
                break;
            
            case GameStates.GameOver:
                onToggleInput?.Invoke(false);
                TriggerPauseMusic?.Invoke();
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
        
        Debug.Log("Keys restored");
        
        Application.Quit();
    }
}
