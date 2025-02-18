using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

// TODO: JumpSound plays on button press inGame. Find out why!
// TODO: Add sounds to gameplay scene!

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
    [SerializeField]private AudioSource startButtonSound;
    [SerializeField]private AudioSource menuButtonSound;
    [SerializeField]private AudioSource backButtonSound;

    private float storedGameMusicTime = 0f;
    
    private const float pitchVarLow = 0.9f;
    private const float pitchVarHigh = 1.1f;
    

    private Coroutine crossFade;
    
    [SerializeField]
    private float fadeDuration = 1.5f;

    private void Awake()
    {
        var existingSoundInstance = FindFirstObjectByType<SoundManager>();
        
        if (existingSoundInstance != null && existingSoundInstance != this)
        {
            Debug.Log("Sound Manager already exists, yikes. Removing it!");
            Destroy(gameObject);
            return;
        }

        menuMusic.volume = 1;
        menuMusic.Play();
        gameMusic.volume = 0;
        
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        HighScoreManager.TriggerHighScoreSound += PlayHighScoreSound;
        PlayerCollision.TriggerPickUpSound += PlayPickUpSound;
        PlayerCollision.OnPlayerDeath += PlayGameOverSound;
        PlayerCollision.OnLifeLost += PlayLifeLostSound;
        JumpMechanic.OnJump += PlayJumpSound;
        
        GameManager.TriggerGameMusic += PlayGameMusic;
        GameManager.TriggerMenuMusic += PlayMenuMusic;
        
        InGameStatesHandler.OnPauseGame += PauseMusic;
        InGameStatesHandler.OnResumeGame+= ResumeMusic;
        InGameStatesHandler.OnQuitToMainMenu += PlayMenuMusic;
    }

    #region Music

    private void PlayMenuMusic()
    {
        StartCrossFade(menuMusic, gameMusic);
    }

    private void PlayGameMusic()
    {
        Debug.Log("Playing Game Music");
        StartCrossFade(gameMusic, menuMusic);
    }
    
    private void StartCrossFade(AudioSource fadeIn, AudioSource fadeOut)
    {
        if (crossFade != null)
        {
            StopCoroutine(crossFade);
        }
        
        fadeIn.volume = 0;
        fadeOut.volume = 1;
        crossFade = StartCoroutine(FadeTracks(fadeIn, fadeOut));
    }

    private IEnumerator FadeTracks(AudioSource fadeIn, AudioSource fadeOut)
    {
        fadeIn.Play();
        float timer = 0f;

        while (timer < fadeDuration)
        {
            float elapsedTime = timer / fadeDuration;
            fadeIn.volume = Mathf.Lerp(0, 1f, elapsedTime);
            fadeOut.volume = Mathf.Lerp(1, 0f, elapsedTime);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeIn.volume = 1f;
        fadeOut.volume = 0f;
        fadeOut.Stop();
    }

    private void PauseMusic()
    {
        if (gameMusic.isPlaying)
        {
            storedGameMusicTime = gameMusic.time;
            gameMusic.Pause();
        }
    }

    private void ResumeMusic()
    {
        gameMusic.time = storedGameMusicTime;
        gameMusic.Play();
    }

    #endregion

    #region SFX

    private void PlayJumpSound()
    {
        jumpSound.pitch = Random.Range(pitchVarLow, pitchVarHigh);
        jumpSound.Play();
    }
    private void PlayHighScoreSound()
    {
        highScoreSound.Play();
    }

    private void PlayLifeLostSound()
    {
        lifeLostSound.Play();
    }

    private void PlayGameOverSound()
    {
        gameOverSound.Play();
    }

    private void PlayPickUpSound()
    {
        pickUpSound.pitch = Random.Range(pitchVarLow, pitchVarHigh);
        pickUpSound.Play();
    }
    
    #endregion

    #region Buttons
    
    public void PlayStartButtonSound()
    {
        startButtonSound.Play();
    }

    public void PlayMenuButtonSound()
    {
        menuButtonSound.Play();
    }

    public void PlayBackButtonSound()
    {
        backButtonSound.Play();
    }

    #endregion
    
    private void OnDisable()
    {
        HighScoreManager.TriggerHighScoreSound -= PlayHighScoreSound;
        PlayerCollision.TriggerPickUpSound -= PlayPickUpSound;
        PlayerCollision.OnPlayerDeath -= PlayGameOverSound;
        PlayerCollision.OnLifeLost -= PlayLifeLostSound;
        JumpMechanic.OnJump -= PlayJumpSound;
        
        GameManager.TriggerGameMusic -= PlayGameMusic;
        GameManager.TriggerMenuMusic -= PlayMenuMusic;
        
        InGameStatesHandler.OnPauseGame -= PauseMusic;
        InGameStatesHandler.OnResumeGame -= ResumeMusic;
    }
}
