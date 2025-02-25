using UnityEngine;
using UnityEngine.SceneManagement;

using static SoundManager;

// TODO: Animate the panels
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject leaderBoardTable;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject musicCreditsPanel;

    private void Start()
    {
        UiToggle();
    }

    private void OnEnable()
    {
        GameManager.onGameStateChanged += HandleStateChange;
    }

    private void HandleStateChange(GameManager.GameStates state)
    {
        if (state != GameManager.GameStates.MainMenu) 
            return;
        
        UiToggle();
    }

    private void UiToggle()
    {
        menuPanel.SetActive(true);
        leaderBoardTable.SetActive(false);
        creditsPanel.SetActive(false);
        musicCreditsPanel.SetActive(false);
        settingsPanel.transform.localScale = Vector2.zero;
    }

    public void onStartPressed()
    {
        GameManager.gameManagerInstance.ChangeState(GameManager.GameStates.GameLoop);
        Instance.PlayStartButtonSound();
        SceneManager.LoadScene(1);
    }

    public void onHighScorePressed()
    {
        Instance.PlayMenuButtonSound();
        
        if(menuPanel.activeSelf)
            menuPanel.SetActive(false);
        
        leaderBoardTable.SetActive(true);
    }

    public void onSettingsPressed()
    {
        Instance.PlayMenuButtonSound();
        
        if(menuPanel.activeSelf)
            menuPanel.SetActive(false);
        
        settingsPanel.transform.localScale = Vector2.one;
    }

    public void onCreditsPressed()
    {
        Instance.PlayMenuButtonSound();
        
        if(menuPanel.activeSelf)
            menuPanel.SetActive(false);
        
        creditsPanel.SetActive(true);
    }

    public void OnMusicCreditsPressed()
    {
        Instance.PlayMenuButtonSound();
        
        if(creditsPanel.activeSelf)
            creditsPanel.SetActive(false);
        
        musicCreditsPanel.SetActive(true);
    }

    public void OnReturnMusicPanel()
    {
        if(musicCreditsPanel.activeSelf)
            musicCreditsPanel.SetActive(false);
        
        creditsPanel.SetActive(true);
    }

    public void onQuitPressed()
    {
        Instance.PlayMenuButtonSound();
        
        Application.Quit();
        Debug.Log("Quit was pressed");
    }

    public void onReturnPressed()
    {
        menuPanel.SetActive(true);
        
        if(leaderBoardTable.activeSelf)
            leaderBoardTable.SetActive(false);
        
        if(settingsPanel.transform.localScale is { x: > 0.1f, y: > 0.1f })
            settingsPanel.transform.localScale = Vector2.zero; 
        
        if(creditsPanel.activeSelf)
            creditsPanel.SetActive(false);
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged -= HandleStateChange;
    }
}
