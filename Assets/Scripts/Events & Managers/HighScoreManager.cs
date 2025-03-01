using System;
using UnityEngine;
using System.Collections.Generic;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager HSInstance;
    
    public static Action<int> OnNewHighScore;
    public static Action TriggerHighScoreSound;
    
    private readonly List<KeyValuePair<string, int>> highScores = new();
    private const int MaxListedScores = 10;
    private int pendingHighScore;
    
    private void Awake()
    {
        if (HSInstance != null && HSInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        HSInstance = this;
        
        DontDestroyOnLoad(gameObject);
        
        LoadScore();
    }

    private void OnEnable()
    {
        EnterNameManager.OnNameEntered += SaveHighScoreToList;
    }
    


    /*
    public void ClearHighScores()
    {
        PlayerPrefs.DeleteKey("ScoreCount");
        
        for (int i = 0; i < MaxListedScores; i++)
        {
            PlayerPrefs.DeleteKey($"HighScore{i}");
            PlayerPrefs.DeleteKey($"HighScoreName{i}");
        }
    
        PlayerPrefs.Save();
        highScores.Clear();
        Debug.Log("High Score Cleared");
    }
    */

    public void AddHighScore(int newScore)
    {
        LoadScore();

        if (newScore <= 0)
            return;
        
        
        if (highScores.Count < MaxListedScores || newScore > highScores[highScores.Count - 1].Value)
        {
            pendingHighScore = newScore;
            OnNewHighScore?.Invoke(newScore);
            TriggerHighScoreSound?.Invoke();
        }
    }

    private void SaveHighScoreToList(string playerName)
    {

            highScores.Add(new KeyValuePair<string, int>(playerName, pendingHighScore));
            highScores.Sort((a, b ) => b.Value.CompareTo(a.Value));
            
            if (highScores.Count > MaxListedScores)
            {
                highScores.RemoveAt(MaxListedScores);
            }
            
            SaveScores();
            pendingHighScore = 0;
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
            
            if (score > 0)
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
