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
        
        float savedSfxVolume = PlayerPrefs.GetFloat(sfxVolume, startVolume);
        float savedMiscVolume = PlayerPrefs.GetFloat(miscVolume, startVolume);
        float savedMusicVolume = PlayerPrefs.GetFloat(musicVolume, startVolume);
    
        sfxSlider.value = savedSfxVolume;
        miscSlider.value = savedMiscVolume;
        musicSlider.value = savedMusicVolume;
        
        ApplyVolume(savedSfxVolume, sfxToAdjust);
        ApplyVolume(savedMiscVolume, miscToAdjust);
        ApplyVolume(savedMusicVolume, musicToAdjust);
    }

    public void OnPointerUpSFX()
    {
        newVolume = sfxSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", newVolume);
        PlayerPrefs.Save();
        ApplyVolume(newVolume, sfxToAdjust);
        
        valueChangedSfx.PlayOneShot(valueChangedSfx.clip, newVolume);
    }

    public void OnPointerUpMisc()
    {
        newVolume = miscSlider.value;
        PlayerPrefs.SetFloat("MiscVolume", newVolume);
        PlayerPrefs.Save();
        ApplyVolume(newVolume, miscToAdjust);
        
        valueChangedMisc.PlayOneShot(valueChangedMisc.clip, newVolume);
    }

    public void OnPointerUpMusic()
    {
        newVolume = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", newVolume);
        PlayerPrefs.Save();
        ApplyVolume(newVolume, musicToAdjust);
        
        valueChangedMisc.PlayOneShot(valueChangedMisc.clip, newVolume);
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
            source.volume = volume;
        }
    }

    [Button("Clear Slider Values")]
    public void ClearSoundsKey()
    {
        PlayerPrefs.DeleteKey(sfxVolume);
        PlayerPrefs.DeleteKey(miscVolume);
        PlayerPrefs.DeleteKey(musicVolume);
        
        Debug.Log("Slider keys cleared");
    }
}
