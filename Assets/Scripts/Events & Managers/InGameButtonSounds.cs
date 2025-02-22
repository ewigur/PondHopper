using UnityEngine;

using static SoundManager;
public class InGameButtonSounds : MonoBehaviour
{
    public void SubmitButtonSound()
    {
        Instance.PlaySubmitButtonSound();
    }

    public void MenuButtonSound()
    {
        Instance.PlayMenuButtonSound();
    }
}
