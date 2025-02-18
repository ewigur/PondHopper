using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameStatesHandler : MonoBehaviour
{
    public static Action OnResumeGame;
    public static Action OnPauseGame;
    public static Action OnQuitToMainMenu;
    
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    
    private string currentScene;
    
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerCollision.OnPlayerDeath += GameOver;
        GameManager.onGameStateChanged += StateChanger;
    }

    private void StateChanger(GameManager.GameStates state)
    {
        if (state != GameManager.GameStates.GamePlay)
            return;
    }

    public void OnPauseClicked()
    {
        OnPauseGame?.Invoke();
        GameManager.gameManagerInstance.ChangeState(GameManager.GameStates.GamePaused);
        pauseMenu.SetActive(true);
    }

    public void OnResumeClicked()
    {
        OnResumeGame?.Invoke();
        GameManager.gameManagerInstance.ChangeState(GameManager.GameStates.GamePlay);
        pauseMenu.SetActive(false);
    }

    public void onRetryClicked()
    {
        GameManager.gameManagerInstance.ChangeState(GameManager.GameStates.GamePlay);
        SceneManager.LoadScene(currentScene);
    }

    public void OnQuitClicked()
    {
        OnQuitToMainMenu?.Invoke();
        GameManager.gameManagerInstance.ChangeState(GameManager.GameStates.MainMenu);
        SceneManager.LoadScene(0);
    }

    private void GameOver()
    {
        GameManager.gameManagerInstance.ChangeState(GameManager.GameStates.GameOver);
        gameOverMenu.SetActive(true);
    }

    private void OnDisable()
    {
        PlayerCollision.OnPlayerDeath -= GameOver;
        GameManager.onGameStateChanged -= StateChanger;
    }
}
