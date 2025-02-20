using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

using static GameManager;

//TODO: WHY DOES SLIDERS NOT PERSIST?!?!??!
//TODO: Change audio?? on ValueChanged

public class AudioSliderHandler : MonoBehaviour
{
    [Header("Slider Attributes")]
    public Slider sfxSlider;
    public Slider miscSlider;
    
    [SerializeField] private List<AudioSource> sfxToAdjust;
    [SerializeField] private List<AudioSource> miscToAdjust;
    
    [SerializeField] private AudioSource valueChangedSfx;
    [SerializeField] private AudioSource valueChangedMisc;

    private const float startVolume = 1f;
    private float newVolume;
    
    private void Start()
    {
        if (!Mathf.Approximately(PlayerPrefs.GetFloat("SFXVolume"), newVolume) ||
            !Mathf.Approximately(PlayerPrefs.GetFloat("MiscVolume"), newVolume))
        {
            
            PlayerPrefs.SetFloat("SFXVolume", startVolume);
            PlayerPrefs.SetFloat("MiscVolume", startVolume);
            Debug.Log("SFX and Misc Volume:" + PlayerPrefs.GetFloat("SFXVolume") + "," + 
                                               PlayerPrefs.GetFloat("MiscVolume"));
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
        
        valueChangedSfx.PlayOneShot(valueChangedSfx.clip);
    }

    public void OnPointerUpMisc()
    {
        newVolume = miscSlider.value;
        PlayerPrefs.SetFloat("MiscVolume", newVolume);
        PlayerPrefs.Save();
        ApplyVolume(newVolume, miscToAdjust);
        
        valueChangedMisc.PlayOneShot(valueChangedMisc.clip);
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
}
