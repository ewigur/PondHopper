using UnityEngine;

public class InGameButtonSounds : MonoBehaviour
{
    [SerializeField]private AudioSource submitButtonSound;
    [SerializeField]private AudioSource ButtonSound;
    public void PlaySubmitButtonSound()
    {
        submitButtonSound.Play();
    }

    public void MenuButtonSound()
    {
        ButtonSound.Play();
    }
}
