using UnityEngine;
using UnityEngine.SceneManagement;

using static SoundManager;
using static GameManager;

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
        onGameStateChanged += HandleStateChange;
    }

    private void HandleStateChange(GameStates state)
    {
        if (state != GameStates.MainMenu) 
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
        GMInstance.ChangeState(GameStates.GameLoop);
        SMInstance.PlayStartButtonSound();
        SceneManager.LoadScene(1);
    }

    public void onHighScorePressed()
    {
        SMInstance.PlayMenuButtonSound();
        
        if(menuPanel.activeSelf)
            menuPanel.SetActive(false);
        
        leaderBoardTable.SetActive(true);
    }

    public void onSettingsPressed()
    {
        SMInstance.PlayMenuButtonSound();
        
        if(menuPanel.activeSelf)
            menuPanel.SetActive(false);
        
        settingsPanel.transform.localScale = Vector2.one;
    }

    public void onCreditsPressed()
    {
        SMInstance.PlayMenuButtonSound();
        
        if(menuPanel.activeSelf)
            menuPanel.SetActive(false);
        
        creditsPanel.SetActive(true);
    }

    public void OnMusicCreditsPressed()
    {
        SMInstance.PlayMenuButtonSound();
        
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
        SMInstance.PlayMenuButtonSound();
        
        GMInstance.OnApplicationQuit();
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
