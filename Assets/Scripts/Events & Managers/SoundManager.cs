using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Tracks")]
    [SerializeField]private AudioSource menuMusic;
    [SerializeField]private AudioSource gameMusic;
    
    [Header("Action Sounds")]
    [SerializeField]private AudioSource highScoreSound;
    [SerializeField]private AudioSource lifeLostSound;
    [SerializeField]private AudioSource gameOverSound;
    [SerializeField]private AudioSource pickUpSound;
    [SerializeField]private AudioSource jumpSound;
    
    [Header("UI sounds")]
    [SerializeField]private AudioSource submitButtonSound;
    [SerializeField]private AudioSource startButtonSound;
    [SerializeField]private AudioSource menuButtonSound;
    [SerializeField]private AudioSource backButtonSound;
    
    
    private void Awake()
    {
        SoundManager existingSoundInstance = FindFirstObjectByType<SoundManager>();
        
        if (existingSoundInstance != null && existingSoundInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }
    
    
}
