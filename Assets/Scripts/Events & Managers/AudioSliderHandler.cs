using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//TODO: Add bool for music instead

public class AudioSliderHandler : MonoBehaviour
{
    private static AudioSliderHandler instance;
    
    [SerializeField] private List<AudioSource> SFX;
    
    [SerializeField] private Slider SFXSlider;

    private readonly float startVolume = 1f;
    private float newSFXVolume;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        if (SFXSlider != null)
        {
            SFXSlider.value = startVolume;
            SFXSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
        }
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

    public void SetSFXVolume()
    {
        newSFXVolume = SFXSlider.value;
        ApplyVolume(newSFXVolume, SFX);
    }
}