using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: Animate the panels, and keep the settingspanel active for sounds to work!
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject leaderBoardTable;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Awake()
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
    
    public void UiToggle()
    {
        menuPanel.SetActive(true);
        leaderBoardTable.SetActive(false);
        creditsPanel.SetActive(false);
        settingsPanel.transform.localScale = Vector2.zero;
    }

    public void onStartPressed()
    {
        GameManager.gameManagerInstance.ChangeState(GameManager.GameStates.GameLoop);
        SceneManager.LoadScene(1);
    }

    public void onHighScorePressed()
    {
        if(menuPanel.activeSelf)
            menuPanel.SetActive(false);
        
        leaderBoardTable.SetActive(true);
    }

    public void onSettingsPressed()
    {
        if(menuPanel.activeSelf)
            menuPanel.SetActive(false);
        
        settingsPanel.transform.localScale = Vector2.one;
    }

    public void onCreditsPressed()
    {
        if(menuPanel.activeSelf)
            menuPanel.SetActive(false);
        
        creditsPanel.SetActive(true);
    }

    public void onQuitPressed()
    {
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
