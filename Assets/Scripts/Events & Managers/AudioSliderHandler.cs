using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//TODO: Make slider stay alive between scene swaps

public class AudioSliderHandler : MonoBehaviour
{
    [Header("Slider Attributes")]
    [SerializeField] private Slider adjustmentSlider;
    [SerializeField] private List<AudioSource> soundsToAdjust;
    [SerializeField] private AudioSource valueChangedSound;
    

    private readonly float startVolume = 1f;
    private float newVolume;

    private void Start()
    {
        if (adjustmentSlider != null)
        {
            adjustmentSlider.value = startVolume;
        }
    }

    public void OnPointerUp()
    {
        newVolume = adjustmentSlider.value;
        ApplyVolume(newVolume, soundsToAdjust);
        
        valueChangedSound.PlayOneShot(valueChangedSound.clip);
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