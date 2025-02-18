using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using NaughtyAttributes;

public class AudioSliderHandler : MonoBehaviour
{
    private static AudioSliderHandler instance;
    
    [SerializeField] private List<AudioSource> musicScores;
    [SerializeField] private List<AudioSource> SFX;
    
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    /*
    private readonly string musicVolumeKey = "musicVolume";
    private readonly string sfxVolumeKey = "sfxVolume";
    private readonly float defaultMusicVolume = 1f;
    private readonly float defaultSfxVolume = 1f;
    */
    private readonly float startVolume = 100f;
    
    private float newMusicVolume;
    private float newSFXVolume;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        
        DontDestroyOnLoad(gameObject);
            
        //newMusicVolume = defaultMusicVolume;
        //ApplyVolume(newMusicVolume);
    }

    private void Start()
    {
        if (musicSlider != null)
        {
            musicSlider.value = startVolume;
            musicSlider.onValueChanged.AddListener
            (
                delegate
                {
                    SetMusicVolume(); 
                }
            );
        }

        if (SFXSlider != null)
        {
            SFXSlider.value = startVolume;
            SFXSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
        }
    }

    private void ApplyVolume(float volume)
    {
        if(musicScores == null || musicScores.Count == 0)
            return;
        
        
        foreach (var mScore in musicScores)
        {
            mScore.volume = volume;
        }
    }

    /*private void SaveMusicVolume()
    {
        PlayerPrefs.SetFloat(musicVolumeKey, newMusicVolume);
        PlayerPrefs.Save();
    }

    private void SaveSFXVolume()
    {
        PlayerPrefs.SetFloat(sfxVolumeKey, newSFXVolume);
        PlayerPrefs.Save();
    }*/

    public void SetMusicVolume()
    {
        
        foreach (var mScores in musicScores)
        {
            mScores.volume = newMusicVolume;
        }
        
        //SaveMusicVolume(); 
    }

    public void SetSFXVolume()
    {
        foreach (var sfx in SFX)
        {
            if (sfx != null)
                sfx.volume = newSFXVolume;
        }
        
        //SaveSFXVolume();
    }
    
    /*
     [Button("Clear Music Volume")]
    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey(musicVolumeKey);
        PlayerPrefs.DeleteKey(sfxVolumeKey);
        PlayerPrefs.Save();
    }
    */
}
