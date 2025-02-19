using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

//TODO: Add bool for music instead
//TODO: Add slider for button sounds

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
            //adjustmentSlider.onValueChanged.AddListener(delegate { SetVolume(); });
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

    /*
     public void SetVolume()
    {
        newVolume = adjustmentSlider.value;
        ApplyVolume(newVolume, soundsToAdjust);
        
        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            valueChangedSound.Play();
        }
    }
    */
}