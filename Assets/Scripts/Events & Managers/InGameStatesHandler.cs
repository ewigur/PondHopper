using UnityEngine;
using UnityEngine.SceneManagement;

using static GameManager;
public class InGameStatesHandler : MonoBehaviour
{
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
        onGameStateChanged += StateChanger;
    }

    
    private void StateChanger(GameStates state)
    {
        if (state == GameStates.GameLoop)
        {
            if (gameManagerInstance.state != GameStates.GamePaused)
            {
                OnPauseClicked();
            }
        }
    }
    

    public void OnPauseClicked()
    {
        gameManagerInstance.ChangeState(GameStates.GamePaused);
        pauseMenu.SetActive(true);
    }

    public void OnResumeClicked()
    {
        gameManagerInstance.ChangeState(GameStates.GameResumed);
        JumpMechanic.canReceiveInput = true;
        pauseMenu.SetActive(false);
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
        gameOverMenu.SetActive(true);
    }

    private void OnDisable()
    {
        PlayerCollision.OnPlayerDeath -= GameOver;
        onGameStateChanged -= StateChanger;
    }
}
