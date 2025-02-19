using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//TODO: WHY DOES SLIDERS NOT PERSIST?!?!??!

public class AudioSliderHandler : MonoBehaviour
{
    public static AudioSliderHandler Instance;
    
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider miscSlider;
    
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
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            Invoke(nameof(FindSliders), 0.1f); // Wait 0.1s before finding sliders
        }
    }

    private void FindSliders()
    {
        GameObject settingsPanel = GameObject.Find("SettingsPanel");  // Find the settings panel
        
        if (settingsPanel != null)
        {
            bool wasActive = settingsPanel.activeSelf;  // Store the current state
            settingsPanel.SetActive(true);  // Temporarily enable it

            sfxSlider = settingsPanel.transform.Find("SFXSlider")?.GetComponent<Slider>();
            miscSlider = settingsPanel.transform.Find("MiscSlider")?.GetComponent<Slider>();

            Debug.Log($"SFX Slider Found: {sfxSlider != null}");
            Debug.Log($"Misc Slider Found: {miscSlider != null}");

            settingsPanel.SetActive(wasActive);  // Restore original state
        }
        else
        {
            Debug.LogError("Settings Panel not found! Make sure it exists in the scene.");
        }
    }

    public void OnPointerUpSFX()
    {
        newVolume = sfxSlider.value;
        ApplyVolume(newVolume, sfxToAdjust);
        PlayerPrefs.SetFloat("SFXVolume", newVolume);
        PlayerPrefs.Save();
        
        valueChangedSfx.PlayOneShot(valueChangedSfx.clip);
    }

    public void OnPointerUpMisc()
    {
        newVolume = miscSlider.value;
        ApplyVolume(newVolume, miscToAdjust);
        PlayerPrefs.SetFloat("MiscVolume", newVolume);
        PlayerPrefs.Save();
        
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
