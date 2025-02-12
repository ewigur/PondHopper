using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

// TODO: Find why the name promt enables on 0 points (score).
public class HighScoreManager : MonoBehaviour
{
    private static HighScoreManager highScoreManagerInstance;
    
    public static Action<int> OnNewHighScore;
    public static Action OnHighScoreUpdated;
    
    private readonly List<KeyValuePair<string, int>> highScores = new();
    private const int MaxScore = 5;
    private int pendingHighScore;
    
    private void Awake()
    {
        if (highScoreManagerInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            highScoreManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        LoadScore();
    }

    private void OnEnable()
    {
        EnterNameManager.OnNameEntered += SaveHighScoreToList;
    }
    

    [Button("Clear High Scores")]
    public void ClearHighScores()
    {
        PlayerPrefs.DeleteKey("ScoreCount");
        
        for (int i = 0; i < MaxScore; i++)
        {
            PlayerPrefs.DeleteKey($"HighScore{i}");
            PlayerPrefs.DeleteKey($"HighScoreName{i}");
        }
    
        PlayerPrefs.Save();
        highScores.Clear();
        Debug.Log("High Score Cleared");
    }

    public void AddHighScore(int newScore)
    {
        LoadScore();

        if (highScores.Count < MaxScore || newScore > highScores[highScores.Count - 1].Value)
        {
            pendingHighScore = newScore;
            OnNewHighScore?.Invoke(newScore);
        }
    }

    private void SaveHighScoreToList(string playerName)
    {

            highScores.Add(new KeyValuePair<string, int>(playerName, pendingHighScore));
            highScores.Sort((a, b ) => b.Value.CompareTo(a.Value));
            
            if (highScores.Count > MaxScore)
            {
                highScores.RemoveAt(MaxScore);
            }
            
            SaveScores();
            pendingHighScore = 0;
            OnHighScoreUpdated?.Invoke();
    }

    private void SaveScores()
    {
        PlayerPrefs.SetInt("ScoreCount", highScores.Count);

        for (int i = 0; i < highScores.Count; i++)
        {
            PlayerPrefs.SetInt($"HighScore{i}", highScores[i].Value);
            PlayerPrefs.SetString($"HighScoreName{i}", highScores[i].Key);
        }
        
        PlayerPrefs.Save();
    }

    private void LoadScore()
    {
        highScores.Clear();
        int scoreCount = PlayerPrefs.GetInt("ScoreCount", 0);

        for (int i = 0; i < scoreCount; i++)
        {
            int score = PlayerPrefs.GetInt($"HighScore{i}", 0);
            string playerName = PlayerPrefs.GetString($"HighScoreName{i}");
            
            if (score > 0)  // Only add non-zero scores (optional check)
                highScores.Add(new KeyValuePair<string, int>(playerName, score));
        }
    }

    public List<KeyValuePair<string, int>> GetHighScores()
    {
        LoadScore();
        return new List<KeyValuePair<string, int>>(highScores);
    }

    private void OnDisable()
    {
        EnterNameManager.OnNameEntered -= SaveHighScoreToList;
    }
}
