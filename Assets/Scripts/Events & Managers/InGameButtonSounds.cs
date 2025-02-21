using UnityEngine;

using static SoundManager;
public class InGameButtonSounds : MonoBehaviour
{
    private void Start()
    {
        if (Instance == null)
        {
            Debug.LogWarning("Cannot find Sound Manager");
        }
    }

    public void SubmitButtonSound()
    {
        Instance.PlaySubmitButtonSound();
    }

    public void MenuButtonSound()
    {
        Instance.PlayMenuButtonSound();
    }
}
