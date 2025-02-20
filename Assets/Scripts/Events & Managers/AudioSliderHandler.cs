using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//TODO: WHY DOES SLIDERS NOT PERSIST?!?!??!

public class AudioSliderHandler : MonoBehaviour
{
    public static AudioSliderHandler Instance;
    
    [Header("Slider Attributes")]
    public Slider sfxSlider;
    public Slider miscSlider;
    
    [SerializeField] private List<AudioSource> sfxToAdjust;
    [SerializeField] private List<AudioSource> miscToAdjust;
    
    [SerializeField] private AudioSource valueChangedSfx;
    [SerializeField] private AudioSource valueChangedMisc;

    private const float startVolume = 1f;
    private float newVolume;
    
    private void Awake()
    { 
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (sfxSlider != null && miscSlider != null)
        {
            sfxSlider.value = startVolume;
            miscSlider.value = startVolume;
        }
    }

    public void OnPointerUpSFX()
    {
        newVolume = sfxSlider.value;
        ApplyVolume(newVolume, sfxToAdjust);
        
        valueChangedSfx.PlayOneShot(valueChangedSfx.clip);
    }

    public void OnPointerUpMisc()
    {
        newVolume = miscSlider.value;
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
