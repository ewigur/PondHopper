using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public static Action<GameStates> onGameStateChanged;
    
    public enum GameStates
    {
        MainMenu,
        GamePlay,
        GamePaused,
        GameOver,
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            ChangeState(GameStates.MainMenu);
            Debug.Log("Started in Main Menu");
        }
    }

    public GameStates state;

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
                Time.timeScale = 1;
                break;
            
            case GameStates.GamePlay:
                Time.timeScale = 1f;
                break;
            
            case GameStates.GamePaused:
                Time.timeScale = 0f;
                break;
            
            case GameStates.GameOver:
                Time.timeScale = 0f;
                break;
            
        }
    }
}
