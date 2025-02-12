using TMPro;
using UnityEngine;

// TODO: Add name input for highscore
// TODO: Add effects when player gets on HS -leaderboard

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score;
    private string playerName;
    
    private HighScoreManager HsManager;

    private void Start()
    {
        HsManager = FindFirstObjectByType<HighScoreManager>();
        
        if (HsManager == null)
        {
            Debug.LogError("No High Score Manager");
            return;
        }
    }

    private void OnEnable()
    {
        PlayerCollision.OnScoreCollected += ScoreCollected;
        PlayerCollision.OnPlayerDeath += FinalScore;
    }
    
    private void ScoreCollected(PickUpItem pickUpItem)
    {
        score += pickUpItem.value;
        scoreText.text = "Score: " + score;
    }

    private void FinalScore()
    {
        if (HsManager != null)
        {
            HsManager.AddHighScore(score);
        }
    }

    public int GetScore()
    {
        return score;
    }

    private void OnDisable()
    {
        PlayerCollision.OnScoreCollected -= ScoreCollected;
        PlayerCollision.OnPlayerDeath -= FinalScore;
    }
}
