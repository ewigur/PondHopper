using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//TODO: WHY DOES SLIDERS NOT PERSIST?!?!??!
//TODO: Change audio?? on ValueChanged

public class AudioSliderHandler : MonoBehaviour
{
    [Header("Slider Attributes")]
    public Slider sfxSlider;
    public Slider miscSlider;
    
    [SerializeField] private List<AudioSource> sfxToAdjust = new();
    [SerializeField] private List<AudioSource> miscToAdjust = new();
    
    [SerializeField] private AudioSource valueChangedSfx;
    [SerializeField] private AudioSource valueChangedMisc;
    
    private SoundManager soundManager;

    private const float startVolume = 1f;
    private string sfxVolume = "SFXVolume";
    private string miscVolume = "MiscVolume";
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
            sfxToAdjust= soundManager.AddSfxSources();
            Debug.Log("Sound Manager found, sounds added: " + sfxToAdjust.Count);
        }
        
        
        if (!Mathf.Approximately(PlayerPrefs.GetFloat(sfxVolume), newVolume) ||
            !Mathf.Approximately(PlayerPrefs.GetFloat(miscVolume), newVolume))
        {
            
            PlayerPrefs.SetFloat(sfxVolume, startVolume);
            PlayerPrefs.SetFloat(miscVolume, startVolume);
            
            Debug.Log("SFX and Misc Volume:" + PlayerPrefs.GetFloat(sfxVolume) + "," + 
                                               PlayerPrefs.GetFloat(miscVolume));
        }
        
        /*sfxSlider.value = startVolume;
        miscSlider.value = startVolume;*/
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
            Debug.Log("Sourced found at volume: " + volume);
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.DeleteKey("SFXVolume");
        PlayerPrefs.DeleteKey("MiscVolume");
    }
}
