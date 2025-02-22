using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AudioSliderHandler : MonoBehaviour
{
    [Header("Slider Attributes")]
    public Slider sfxSlider;
    public Slider musicSlider;
    public Slider buttonsSlider;
    
    [SerializeField] private List<AudioSource> sfxToAdjust = new();
    [SerializeField] private List<AudioSource> musicToAdjust = new();
    [SerializeField] private List<AudioSource> buttonsToAdjust = new();
    
    [SerializeField] private AudioSource valueChangedSfx;
    [SerializeField] private AudioSource valueChangedButtons;
    
    private SoundManager soundManager;

    private const float startVolume = 1f;
    
    private float savedSfxVolume;
    private float savedMusicVolume;
    private float savedButtonsVolume;
    
    private readonly string sfxVolume = "SFXVolume";
    private readonly string musicVolume = "MusicVolume";
    private readonly string buttonsVolume = "ButtonsVolume";
    
    private float newVolume;
    
    private void Start()
    {
        soundManager = FindFirstObjectByType<SoundManager>();

        if (soundManager == null) 
            return;
        
        sfxToAdjust = soundManager.AddSfxSources();
        musicToAdjust = soundManager.AddMusicSources();
        buttonsToAdjust = soundManager.AddButtonsSources();
            
        SetVolume();
    }

    private void SetVolume()
    {
        if(!PlayerPrefs.HasKey(sfxVolume)) PlayerPrefs.SetFloat(sfxVolume, startVolume);
        if(!PlayerPrefs.HasKey(musicVolume)) PlayerPrefs.SetFloat(musicVolume, startVolume);
        if(!PlayerPrefs.HasKey(buttonsVolume)) PlayerPrefs.SetFloat(buttonsVolume, startVolume);
        
        savedSfxVolume = PlayerPrefs.GetFloat(sfxVolume, startVolume);
        savedMusicVolume = PlayerPrefs.GetFloat(musicVolume, startVolume);
        savedButtonsVolume = PlayerPrefs.GetFloat(buttonsVolume, startVolume);
        
        sfxSlider.value = savedSfxVolume;
        musicSlider.value = savedMusicVolume;
        buttonsSlider.value = savedButtonsVolume;
        
        ApplyVolume(savedSfxVolume, sfxToAdjust);
        ApplyVolume(savedMusicVolume, musicToAdjust);
        ApplyVolume(savedButtonsVolume, buttonsToAdjust);
    }

    public void OnPointerUpSFX()
    {
        newVolume = sfxSlider.value;
        PlayerPrefs.SetFloat(sfxVolume, newVolume);
        ApplyVolume(newVolume, sfxToAdjust);
        
        valueChangedSfx.PlayOneShot(valueChangedSfx.clip, newVolume);
        
        SavePreferences();
    }

    public void OnPointerUpButtons()
    {
        newVolume = buttonsSlider.value;
        PlayerPrefs.SetFloat(buttonsVolume, newVolume);
        ApplyVolume(newVolume, buttonsToAdjust);
        
        valueChangedButtons.PlayOneShot(valueChangedButtons.clip, newVolume);
        
        SavePreferences();
    }

    public void OnPointerUpMusic()
    {
        newVolume = musicSlider.value;
        PlayerPrefs.SetFloat(musicVolume, newVolume);
        
        ApplyVolume(newVolume, musicToAdjust);

        if (soundManager != null)
        {
            soundManager.SetMusicVolume(newVolume);
        }
        
        SavePreferences();
    }

    private void ApplyVolume(float volume, List<AudioSource> sources)
    {
        if (sources == null || sources.Count == 0)
            return;
        
        
        foreach (var source in sources)
        {
            source.volume = volume;
        }
    }

    private void SavePreferences()
    {
        PlayerPrefs.Save();
    }
}
