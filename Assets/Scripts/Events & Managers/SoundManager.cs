using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

using static HighScoreManager;
using static GameManager;

//TODO: Fix AddSource Method (getting nulls)

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Tracks")] 
    [SerializeField]private AudioSource menuMusic;
    [SerializeField]private AudioSource gameMusic;
    [SerializeField]private AudioSource pauseAmbience;

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
    [SerializeField]private AudioSource submitButtonSound;

    [SerializeField] private float fadeDuration = 1.5f;
    
    private List<AudioSource> trackSources;
    private List<AudioSource> sfxSources;
    private List<AudioSource> miscSources;
    
    private const float pitchVarLow = 0.9f;
    private const float pitchVarHigh = 1.1f;
    
    private AudioSliderHandler audioHandler;
    private Coroutine crossFade;

    private void Awake()
    {
        pauseAmbience.Stop();
        pauseAmbience.volume = 0.001f;
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(this);
        
        sfxSources = new List<AudioSource>();
        trackSources = new List<AudioSource>();
        miscSources = new List<AudioSource>();
    }

    private void Start()
    {
        menuMusic.volume = 1;
        menuMusic.Play();
        gameMusic.volume = 0;
    }

    private void OnEnable()
    {
        PlayerCollision.TriggerPickUpSound += PlayPickUpSound;
        PlayerCollision.OnPlayerDeath += PlayGameOverSound;
        PlayerCollision.OnLifeLost += PlayLifeLostSound;
        JumpMechanic.OnJump += PlayJumpSound;
        
        TriggerGameMusic += PlayGameMusic;
        TriggerMenuMusic += PlayMenuMusic;
        TriggerPauseMusic += PlayPauseAmbience;
        TriggerResumeMusic += ResumeGameMusic;
        TriggerHighScoreSound += PlayHighScoreSound;
    }

    #region Music

    private bool CheckBool()
    {
        if (MusicBool.Instance == null)
        {
            Debug.LogWarning("Can't find MusicBool");
            return false;
        }
        
        Debug.Log("Returning MusicBool");
        return MusicBool.Instance.musicIsOn;
    }
    private void PlayMenuMusic()
    {
        if (!CheckBool()) 
            return;
        
        StartCrossFade(menuMusic, gameMusic);
        pauseAmbience.Stop();
    }

    private void PlayGameMusic()
    {
        if (!CheckBool())
            return;
        
        
        StartCrossFade(gameMusic, menuMusic);
        pauseAmbience.Stop();
    }
    
    private void PlayPauseAmbience()
    {
        if (!CheckBool())
            return;

        if (gameManagerInstance.state != GameStates.GamePaused &&
            gameManagerInstance.state != GameStates.GameOver) return;
        
        gameMusic.Pause();
        pauseAmbience.Play();
    }
    private void ResumeGameMusic()
    {
        if (!CheckBool())
            return;
        
        
        gameMusic.UnPause();
        pauseAmbience.Stop();
    }
    
    private void StartCrossFade(AudioSource fadeIn, AudioSource fadeOut)
    {
        if (crossFade != null)
        {
            StopCoroutine(crossFade);
        }

        if (audioHandler != null)
        {
            fadeIn.volume = 0;
            fadeOut.volume = 1;
        }
        
        crossFade = StartCoroutine(FadeTracks(fadeIn, fadeOut));
    }

    private IEnumerator FadeTracks(AudioSource fadeIn, AudioSource fadeOut)
    {
        fadeIn.Play();
        
        var timer = 0f;

        while (timer < fadeDuration)
        {
            var elapsedTime = timer / fadeDuration;
            fadeIn.volume = Mathf.Lerp(0, 1f, elapsedTime);
            fadeOut.volume = Mathf.Lerp(1, 0f, elapsedTime);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeIn.volume = 1f;
        fadeOut.volume = 0f;
        fadeOut.Stop();
    }
    #endregion

    #region SFX

    public List<AudioSource> AddSfxSources()
    {
        SfxSources(highScoreSound);
        SfxSources(lifeLostSound);
        SfxSources(gameOverSound);
        SfxSources(pickUpSound);
        SfxSources(jumpSound);
        
        return sfxSources;
    }

    private void SfxSources(AudioSource source)
    {
        if (source != null && !sfxSources.Contains(source))
        {
            sfxSources.Add(source);
        }
    }
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
    
    public List<AudioSource> AddMiscSources()
    {
        MiscSources(startButtonSound);
        MiscSources(menuButtonSound);
        MiscSources(backButtonSound);
        MiscSources(submitButtonSound);
        
        return miscSources;
    }

    private void MiscSources(AudioSource source)
    {
        if (source != null && !miscSources.Contains(source))
        {
            miscSources.Add(source);
        }
    }

    public void PlaySubmitButtonSound()
    {
        submitButtonSound.Play();
    }
    
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
        PlayerCollision.TriggerPickUpSound -= PlayPickUpSound;
        PlayerCollision.OnPlayerDeath -= PlayGameOverSound;
        PlayerCollision.OnLifeLost -= PlayLifeLostSound;
        JumpMechanic.OnJump -= PlayJumpSound;
        
        TriggerGameMusic -= PlayGameMusic;
        TriggerMenuMusic -= PlayMenuMusic;
        TriggerPauseMusic -= PlayPauseAmbience;
        TriggerResumeMusic -= ResumeGameMusic;
        TriggerHighScoreSound -= PlayHighScoreSound;
    }
}
