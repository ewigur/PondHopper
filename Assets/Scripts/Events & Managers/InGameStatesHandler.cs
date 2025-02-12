using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameStatesHandler : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject frogLr;
    
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
        {
            Debug.Log("Game play state is " + state);
            return;
        }
        
        Debug.Log("InGame State");
    }

    public void OnPauseClicked()
    {
        GameManager.instance.ChangeState(GameManager.GameStates.GamePaused);
        frogLr.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void OnResumeClicked()
    {
        GameManager.instance.ChangeState(GameManager.GameStates.GamePlay);
        frogLr.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void onRetryClicked()
    {
        GameManager.instance.ChangeState(GameManager.GameStates.GamePlay);
        SceneManager.LoadScene(currentScene);
        
    }

    public void OnQuitClicked()
    {
        GameManager.instance.ChangeState(GameManager.GameStates.MainMenu);
        SceneManager.LoadScene(0);
    }

    private void GameOver()
    {
        GameManager.instance.ChangeState(GameManager.GameStates.GameOver);
        frogLr.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    private void OnDisable()
    {
        PlayerCollision.OnPlayerDeath -= GameOver;
        GameManager.onGameStateChanged -= StateChanger;
    }
}
