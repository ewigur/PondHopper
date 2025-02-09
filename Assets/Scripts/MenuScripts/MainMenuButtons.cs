using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject menuPanel;
    
    
    public void onStartPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void onHighScorePressed()
    {
        if(menuPanel.activeSelf)
            menuPanel.SetActive(false);
    }

    public void onSettingsPressed()
    {

    }

    public void onCreditsPressed()
    {

    }

    public void onQuitPressed()
    {
        Application.Quit();
        Debug.Log("Quit was pressed");
    }

    public void onReturnPressed()
    {

    }
}
