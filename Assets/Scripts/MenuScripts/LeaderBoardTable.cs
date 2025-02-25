using System.Collections.Generic;
using UnityEngine;
public class LeaderBoardTable : MonoBehaviour
{
    [SerializeField] private float tempHeight = 30f;
    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryTemplate;
    
    private HighScoreManager highScoreManager;
    private const int maxEntries = 8;

    private void Start()
    {
        highScoreManager = FindFirstObjectByType<HighScoreManager>();

        if (highScoreManager == null)
            return;

        DisplayLeaderBoard();
    }
    
    private void DisplayLeaderBoard()
    {
        foreach (Transform child in entryContainer)
        {
            if (child != entryTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        List<KeyValuePair<string, int>> highScores = highScoreManager.GetHighScores();
        
        entryTemplate.gameObject.SetActive(true);
        
        for (int i = 0; i < highScores.Count && i < maxEntries; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRect = entryTransform.GetComponent<RectTransform>();
            
            entryRect.anchoredPosition = new Vector2(0, -tempHeight * i);
        
            entryTransform.gameObject.SetActive(true);

            int rank = i + 1;
            
            string rankTag = rank switch
            {
                1 => "ST",
                2 => "ND",
                3 => "RD",
                _ => "TH"
            };
            
            string rankText = rank + rankTag;
            
            var posCountText = entryTransform.Find("PosCountText")?.GetComponent<TMPro.TextMeshProUGUI>();
            var scoreCountText = entryTransform.Find("ScoreCountText")?.GetComponent<TMPro.TextMeshProUGUI>();
            var PlayerName = entryTransform.Find("PlayerNameText")?.GetComponent<TMPro.TextMeshProUGUI>();

            if (posCountText != null)
            {
                posCountText.text = rankText;
            }

            if (scoreCountText != null)
            {
                scoreCountText.text = highScores[i].Value.ToString();
            }

            if (PlayerName != null)
            {
                PlayerName.text = highScores[i].Key;
            }
        }
    }
}
