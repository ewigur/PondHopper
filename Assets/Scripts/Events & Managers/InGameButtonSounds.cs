using UnityEngine;

using static SoundManager;
public class InGameButtonSounds : MonoBehaviour
{
    public void SubmitButtonSound()
    {
        SMInstance.PlaySubmitButtonSound();
    }

    public void MenuButtonSound()
    {
        SMInstance.PlayMenuButtonSound();
    }
}
