using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//TODO: Make toggle stay alive between scene swaps
public class MusicBool : MonoBehaviour
{
    public static MusicBool Instance;
    
    public bool musicIsOn = true;
    
    [Header("Toggler Attributes")]
    public Toggle musicToggle;
    [SerializeField] private List<AudioSource> musicSources;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (musicToggle != null)
        {
            musicToggle.isOn = musicIsOn;
            musicToggle.onValueChanged.AddListener(ToggleMusic);
        }

        ToggleMusic(musicIsOn);
    }

    public void ToggleMusic(bool isMusicOn)
    {
        musicIsOn = isMusicOn;

        foreach (var source in musicSources)
        {
            if (musicIsOn)
                source.Play();
            else
                source.Stop();
        }
    }
}