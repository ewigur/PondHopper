using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        Debug.Log("Change to MainMenu State");
        
        UiToggle();
    }
    
    public void UiToggle()
    {
        menuPanel.SetActive(true);
        leaderBoardTable.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
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
        
        settingsPanel.SetActive(true);
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
        
        if(settingsPanel.activeSelf)
            settingsPanel.SetActive(false);
        
        if(creditsPanel.activeSelf)
            creditsPanel.SetActive(false);
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged -= HandleStateChange;
    }
}
