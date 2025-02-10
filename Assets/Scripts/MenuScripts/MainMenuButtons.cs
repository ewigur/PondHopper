using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject leaderBoardTable;

    private void Awake()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
        }
        
        if (leaderBoardTable != null)
        {
            leaderBoardTable.SetActive(false);
        }
    }


    public void onStartPressed()
    {
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
        Debug.Log("Settings was pressed");
    }

    public void onCreditsPressed()
    {
        Debug.Log("Credits was pressed");
    }

    public void onQuitPressed()
    {
        Application.Quit();
        Debug.Log("Quit was pressed");
    }

    public void onReturnPressed()
    {
        menuPanel.SetActive(true);
        leaderBoardTable.SetActive(false);
    }
}
