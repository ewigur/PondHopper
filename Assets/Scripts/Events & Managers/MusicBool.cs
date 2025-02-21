using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System.Collections.Generic;

using static SoundManager;
using static GameManager;

//TODO: Make toggle stay alive between scene swaps
public class MusicBool : MonoBehaviour
{
    public static MusicBool MusicBoolInstance;
    
    [Header("Toggler Attributes")]
    public Toggle musicToggle;
    public bool musicIsOn = true;
    
    private readonly string musicEnabled = "MusicEnabled";
    private List<AudioSource> musicSources = new();
    private bool isToggleFound = false;
    
    private void Awake()
    {
        if (MusicBoolInstance != null && MusicBoolInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        MusicBoolInstance = this;
        DontDestroyOnLoad(gameObject);

        musicSources = Instance.AddMusicSources();
        Debug.Log("Added Music Sources" + nameof(musicSources));
    }

    private void OnEnable()
    {
        if (gameManagerInstance.state == GameStates.MainMenu)
        {
            MainMenuManager.OnSettingsActivated += FindToggle;
        }
    }

    private void FindToggle()
    {
        if (isToggleFound)
        {
            Debug.Log("Music Toggle already found");
            return;
        }
        
        musicToggle = FindFirstObjectByType<Toggle>();

        if (musicToggle != null)
        {
            Debug.Log("Music Toggle Found!");
            musicToggle.onValueChanged.AddListener(ToggleMusic);
            musicToggle.isOn = musicIsOn;
            isToggleFound = true;
        }
    }

    private void Start()
    {
        ToggleMusic(musicIsOn);
    }
    
    public void ToggleMusic(bool isMusicOn)
    {
        musicIsOn = isMusicOn;
        PlayerPrefs.SetInt(musicEnabled, musicIsOn ? 1 : 0);
        PlayerPrefs.Save();

        foreach (var source in musicSources)
        {
            if (source != null)
            {
                if (!musicIsOn)
                    source.Pause();
                else
                    source.UnPause();
            }
        }
    }

    private void OnDisable()
    {
        MainMenuManager.OnSettingsActivated -= FindToggle;
    }

    /*private void OnDestroy()
    {
        if (isToggleFound && MusicBoolInstance != null)
        {
            musicToggle.onValueChanged.RemoveListener(ToggleMusic);
        }
    }
    */

    [Button("Clear Music Toggle")]
    public void ClearMusicKey()
    {
        PlayerPrefs.DeleteKey(musicEnabled);
        
        Debug.Log("Music key cleared");
    }
}