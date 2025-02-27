using TMPro;
using UnityEngine;

using static HighScoreManager;
using static PlayerHealth;
using static GameManager;
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    
    private int score;

    private void Start()
    {
        if (GMInstance.state == GameStates.GameRestarted)
        {
            SavedScore();
        }

        else
        {
            PlayerPrefs.SetInt("currentScore", 0);
            score = PlayerPrefs.GetInt("currentScore", score);
        }

        scoreText.text = "Score: " + score;
    }

    private void OnEnable()
    {
        PlayerCollision.OnScoreCollected += ScoreCollected;
        PlayerHasDied += FinalScore;
        OnLifeLost += SavedScore;
    }

    private void ScoreCollected(PickUpItem pickUpItem)
    {
        score += pickUpItem.value;
        scoreText.text = "Score: " + score;
        PlayerPrefs.SetInt("currentScore", score);
    }

    private void SavedScore()
    {
        score = PlayerPrefs.GetInt("currentScore", 0);
        scoreText.text = "Score: " + score;
    }

    private void FinalScore()
    {
        if ( HSInstance != null)
        {
            HSInstance.AddHighScore(score);
        }
    }

    private void OnDisable()
    {
        PlayerCollision.OnScoreCollected -= ScoreCollected;
        PlayerHasDied -= FinalScore;
        OnLifeLost -= SavedScore;
    }
}