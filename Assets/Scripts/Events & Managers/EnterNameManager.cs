using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EnterNameManager : MonoBehaviour
{
    public static Action<string> OnNameEntered;
    
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private GameObject namePrompt;
    [SerializeField] private Button doneButton;
    
    private const int maxNameLength = 10;

    private int currentScore;

    private void Start()
    {
        namePrompt.SetActive(false);
        
        nameInputField.characterLimit = maxNameLength;
        doneButton.interactable = false;
    }

    private void OnEnable()
    {
        HighScoreManager.OnNewHighScore += ShowNamePrompt;
        nameInputField.onValueChanged.AddListener(ValidateInput);
    }

    private void ShowNamePrompt(int score)
    {
        if (!namePrompt.activeSelf)
        {
            namePrompt.SetActive(true);
            nameInputField.text = "";
            doneButton.interactable = false;
            nameInputField.Select();
        }
    }

    private void ValidateInput(string input)
    {
        doneButton.interactable = !string.IsNullOrEmpty(input);
    }

    public void SubmitName()
    {
        string playerName = nameInputField.text.Trim();

        if (!string.IsNullOrEmpty(playerName))
        {
            OnNameEntered?.Invoke(playerName);
            namePrompt.SetActive(false);
        }
    }

    private void OnDisable()
    {
        HighScoreManager.OnNewHighScore -= ShowNamePrompt;
        nameInputField.onValueChanged.RemoveListener(ValidateInput);
    }
}
