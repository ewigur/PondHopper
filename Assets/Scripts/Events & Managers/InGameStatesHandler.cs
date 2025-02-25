using UnityEngine;
using UnityEngine.SceneManagement;

using static GameManager;
public class InGameStatesHandler : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject pauseButton;
    
    private string currentScene;
    
    private void Start()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        pauseButton.SetActive(true);
        currentScene = SceneManager.GetActiveScene().name;
    }

    private void OnEnable()
    {
        PlayerCollision.OnPlayerDeath += GameOver;
    }
    public void OnPauseClicked()
    {
        gameManagerInstance.ChangeState(GameStates.GamePaused);
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void OnResumeClicked()
    {
        gameManagerInstance.ChangeState(GameStates.GameResumed);
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void onRetryClicked()
    {
        gameManagerInstance.ChangeState(GameStates.GameLoop);
        SceneManager.LoadScene(currentScene);
    }

    public void OnQuitClicked()
    {
        gameManagerInstance.ChangeState(GameStates.MainMenu);
        SceneManager.LoadScene(0);
    }

    private void GameOver()
    {
        gameManagerInstance.ChangeState(GameStates.GameOver);
        pauseButton.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    private void OnDisable()
    {
        PlayerCollision.OnPlayerDeath -= GameOver;
    }
}
