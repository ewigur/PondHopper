using UnityEngine;
using UnityEngine.SceneManagement;

using static HealthManager;
using static GameManager;
using static PlayerHealth;

public class InGameStatesHandler : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject livesDisplay;
    
    private string currentScene;
    
    private void Start()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseButton.SetActive(true);
        livesDisplay.SetActive(true);
        currentScene = SceneManager.GetActiveScene().name;
    }

    private void OnEnable()
    {
        OnLifeLost += GameRestart;
        PlayerHasDied += GameOver;
    }

    public void OnPauseClicked()
    {
        GMInstance.ChangeState(GameStates.GamePaused);
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void OnResumeClicked()
    {
        GMInstance.ChangeState(GameStates.GameResumed);
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
    }
    
    private void GameRestart()
    {
        GMInstance.ChangeState(GameStates.GameRestarted);
        SceneManager.LoadScene(currentScene);
        remainingLives = PlayerPrefs.GetInt("remainingLives", PHInstance.maxLives);
        HMInstance.RestoreHealthUI();
    }

    public void onRetryClicked()
    {
        GMInstance.ChangeState(GameStates.GameLoop);
        SceneManager.LoadScene(currentScene);
    }

    public void OnQuitClicked()
    {
        GMInstance.ChangeState(GameStates.MainMenu);
        SceneManager.LoadScene(0);
    }

    private void GameOver()
    {
        GMInstance.ChangeState(GameStates.GameOver);
        livesDisplay.SetActive(false);
        pauseButton.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    private void OnDisable()
    {
        OnLifeLost -= GameRestart;
        PlayerHasDied -= GameOver;
    }
}
