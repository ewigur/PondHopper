using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

using static HighScoreManager;
using static PlayerHealth;
using static GameManager;

public class SoundManager : MonoBehaviour
{
    public static SoundManager SMInstance;

    [Header("Tracks")] 
    [SerializeField]private AudioSource pauseAmbience;
    [SerializeField]private AudioSource menuMusic;
    [SerializeField]private AudioSource gameMusic;

    [Header("SFX Sounds")] 
    [SerializeField]private AudioSource specialPickUpSound;
    [SerializeField]private AudioSource frogCroakSound;
    [SerializeField]private AudioSource highScoreSound;
    [SerializeField]private AudioSource lifeLostSound;
    [SerializeField]private AudioSource gameOverSound;
    [SerializeField]private AudioSource pickUpSound;
    [SerializeField]private AudioSource jumpSound;
    
    [Header("Button sounds")]
    [SerializeField]private AudioSource submitButtonSound;
    [SerializeField]private AudioSource startButtonSound;
    [SerializeField]private AudioSource menuButtonSound;
    [SerializeField]private AudioSource backButtonSound;

    [SerializeField] private float fadeDuration = 1.5f;
    
    private List<AudioSource> sfxSources;
    private List<AudioSource> musicSources;
    private List<AudioSource> buttonsSources;
    
    private readonly float fadeOutVolume = 0.001f;
    private const float pitchVarHigh = 1.1f;
    private const float pitchVarLow = 0.9f;
    
    private AudioSliderHandler audioHandler;
    private Coroutine crossFade;

    private void Awake()
    {
        pauseAmbience.Stop();
        
        if (SMInstance != null && SMInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        SMInstance = this;
        DontDestroyOnLoad(this);
        
        sfxSources = new List<AudioSource>();
        musicSources = new List<AudioSource>();
        buttonsSources = new List<AudioSource>();
    }

    private void Start()
    {
        if (musicSources == null || musicSources.Count == 0)
        {
            AddMusicSources();
        }
        
        menuMusic.Play();
    }

    private void OnEnable()
    {
        PlayerCollision.TriggerSpecialPickUpSound += PlaySpecialPickUpSound;
        PlayerCollision.TriggerPickUpSound += PlayPickUpSound;
        JumpMechanic.OnPreJump += PlayCroakSound;
        JumpMechanic.OnJump += PlayJumpSound;
        
        TriggerHighScoreSound += PlayHighScoreSound;
        TriggerPauseMusic += PlayPauseAmbience;
        TriggerResumeMusic += ResumeGameMusic;
        TriggerMenuMusic += PlayMenuMusic;
        TriggerGameMusic += PlayGameMusic;
        PlayerHasDied += PlayGameOverSound;
        OnLifeLost += PlayLifeLostSound;
    }

    #region Music
    
    public List<AudioSource> AddMusicSources()
    {
        MusicSources(menuMusic);
        MusicSources(gameMusic);
        MusicSources(pauseAmbience);
        
        return musicSources;
    }

    private void MusicSources(AudioSource source)
    {
        if (source == null)
        {
            return;
        }
        
        if (!musicSources.Contains(source))
        {
            musicSources.Add(source);
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSources == null || musicSources.Count == 0)
        {
            AddMusicSources();
        }

        else
        {
            foreach (var source in musicSources)
            {
                source.volume = volume;
            }
        }
    }
    
    private void PlayMenuMusic()
    {
        StartCrossFade(menuMusic, gameMusic);
        pauseAmbience.Stop();
    }

    private void PlayGameMusic()
    {
        StartCrossFade(gameMusic, menuMusic);
        pauseAmbience.Stop();
    }
    
    private void PlayPauseAmbience()
    {
        if (GMInstance.state != GameStates.GamePaused &&
            GMInstance.state != GameStates.GameOver) 
            return;
        
        gameMusic.Pause();
        pauseAmbience.Play();
    }
    private void ResumeGameMusic()
    {
        gameMusic.UnPause();
        pauseAmbience.Stop();
    }
    
    private void StartCrossFade(AudioSource fadeIn, AudioSource fadeOut)
    {
        if (crossFade != null)
        {
            StopCoroutine(crossFade);
        }
        
        float currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        
        fadeIn.volume = currentMusicVolume;
        fadeOut.volume = fadeOutVolume;
            
        crossFade = StartCoroutine(FadeTracks(fadeIn, fadeOut));
    }

    private IEnumerator FadeTracks(AudioSource fadeIn, AudioSource fadeOut)
    {
        fadeIn.Play();
        
        var targetVolume = PlayerPrefs.GetFloat("MusicVolume");
        var timer = 0f;
        
        while (timer < fadeDuration)
        {
            var elapsedTime = timer / fadeDuration;
            fadeIn.volume = Mathf.Lerp(fadeOutVolume, targetVolume, elapsedTime);
            fadeOut.volume = Mathf.Lerp(targetVolume, fadeOutVolume, elapsedTime);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeIn.volume = targetVolume;
        fadeOut.volume = fadeOutVolume;
        fadeOut.Stop();
    }
    #endregion

    #region SFX

    public List<AudioSource> AddSfxSources()
    {
        SfxSources(specialPickUpSound);
        SfxSources(frogCroakSound);
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
    
    private void PlaySpecialPickUpSound()
    {
        specialPickUpSound.pitch = Random.Range(pitchVarLow, pitchVarHigh);
        specialPickUpSound.Play();
    }
    
    private void PlayCroakSound()
    {
        frogCroakSound.pitch = Random.Range(pitchVarLow, pitchVarHigh);
        frogCroakSound.Play();
    }

    #endregion

    #region Buttons
    
    public List<AudioSource> AddButtonsSources()
    {
        ButtonSources(startButtonSound);
        ButtonSources(menuButtonSound);
        ButtonSources(backButtonSound);
        ButtonSources(submitButtonSound);
        
        return buttonsSources;
    }

    private void ButtonSources(AudioSource source)
    {
        if (source != null && !buttonsSources.Contains(source))
        {
            buttonsSources.Add(source);
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
        PlayerCollision.TriggerSpecialPickUpSound -= PlaySpecialPickUpSound;
        PlayerCollision.TriggerPickUpSound -= PlayPickUpSound;
        JumpMechanic.OnPreJump -= PlayCroakSound;
        JumpMechanic.OnJump -= PlayJumpSound;
        
        TriggerHighScoreSound -= PlayHighScoreSound;
        TriggerPauseMusic -= PlayPauseAmbience;
        TriggerResumeMusic -= ResumeGameMusic;
        TriggerMenuMusic -= PlayMenuMusic;
        TriggerGameMusic -= PlayGameMusic;
        PlayerHasDied -= PlayGameOverSound;
        OnLifeLost -= PlayLifeLostSound;
    }
}
