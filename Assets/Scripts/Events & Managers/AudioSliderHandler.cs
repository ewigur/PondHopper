using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine;

//TODO: Fix Music volume instance (no value changing with slider)

public class AudioSliderHandler : MonoBehaviour
{
    [Header("Slider Attributes")]
    public Slider sfxSlider;
    public Slider miscSlider;
    public Slider musicSlider;
    
    [SerializeField] private List<AudioSource> sfxToAdjust = new();
    [SerializeField] private List<AudioSource> miscToAdjust = new();
    [SerializeField] private List<AudioSource> musicToAdjust = new();
    
    [SerializeField] private AudioSource valueChangedSfx;
    [SerializeField] private AudioSource valueChangedMisc;
    
    private SoundManager soundManager;

    private const float startVolume = 1f;
    
    private float savedSfxVolume;
    private float savedMiscVolume;
    private float savedMusicVolume;
    
    private readonly string sfxVolume = "SFXVolume";
    private readonly string miscVolume = "MiscVolume";
    private readonly string musicVolume = "MusicVolume";
    
    private float newVolume;
    
    private void Start()
    {
        soundManager = FindFirstObjectByType<SoundManager>();

        if (soundManager == null)
        {
            Debug.LogError("Sound Manager not found");
        }
        else
        {
            sfxToAdjust = soundManager.AddSfxSources();
            miscToAdjust = soundManager.AddMiscSources();
            musicToAdjust = soundManager.AddMusicSources();
        }
        
        if(!PlayerPrefs.HasKey(sfxVolume)) PlayerPrefs.SetFloat(sfxVolume, startVolume);
        if(!PlayerPrefs.HasKey(miscVolume)) PlayerPrefs.SetFloat(miscVolume, startVolume);
        if(!PlayerPrefs.HasKey(musicVolume)) PlayerPrefs.SetFloat(musicVolume, startVolume);
        
        savedSfxVolume = PlayerPrefs.GetFloat(sfxVolume, startVolume);
        savedMiscVolume = PlayerPrefs.GetFloat(miscVolume, startVolume);
        savedMusicVolume = PlayerPrefs.GetFloat(musicVolume, startVolume);
        
        sfxSlider.value = savedSfxVolume;
        miscSlider.value = savedMiscVolume;
        musicSlider.value = savedMusicVolume;
        
        ApplyVolume(savedSfxVolume, sfxToAdjust);
        ApplyVolume(savedMiscVolume, miscToAdjust);
        ApplyVolume(savedMusicVolume, musicToAdjust);
        
        SavePreferences();
    }

    public void OnPointerUpSFX()
    {
        newVolume = sfxSlider.value;
        PlayerPrefs.SetFloat(sfxVolume, newVolume);
        ApplyVolume(newVolume, sfxToAdjust);
        
        valueChangedSfx.PlayOneShot(valueChangedSfx.clip, newVolume);
        
        SavePreferences();
    }

    public void OnPointerUpMisc()
    {
        newVolume = miscSlider.value;
        PlayerPrefs.SetFloat(miscVolume, newVolume);
        ApplyVolume(newVolume, miscToAdjust);
        
        valueChangedMisc.PlayOneShot(valueChangedMisc.clip, newVolume);
        
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

        else
        {
            Debug.LogError("Sound Manager not found");
        }
        
        SavePreferences();
    }

    private void ApplyVolume(float volume, List<AudioSource> sources)
    {
        if (sources == null || sources.Count == 0)
        {
            Debug.LogWarning("No audio sources assigned to AudioSliderHandler");
            return;
        }
        
        foreach (var source in sources)
        {
            Debug.Log("Applying volume to " + source.name + ": " + volume);
            source.volume = volume;
        }
    }

    private void SavePreferences()
    {
        PlayerPrefs.Save();
        Debug.Log("Saved Preferences");
    }

    [Button("Clear Slider Values")]
    public void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey(sfxVolume);
        PlayerPrefs.DeleteKey(miscVolume);
        PlayerPrefs.DeleteKey(musicVolume);
        
        PlayerPrefs.SetFloat(sfxVolume, startVolume);
        PlayerPrefs.SetFloat(miscVolume, startVolume);
        PlayerPrefs.SetFloat(musicVolume, startVolume);
        PlayerPrefs.Save();
        
        Debug.Log("Slider keys cleared, volumes set to default");
    }
}
