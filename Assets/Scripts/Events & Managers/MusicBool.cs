using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicBool : MonoBehaviour
{
    private bool musicOn = true;
    
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private List<AudioSource> musicSources;
    

    public bool ToggleMusic()
    {
        if (musicToggle.isOn)
        {
            foreach (AudioSource source in musicSources)
            {
                musicOn = true;
            }
            return true;
        }
        
        //else
        {
            foreach (AudioSource source in musicSources)
            {
                source.Pause();
                musicOn = false;
            }
            return false;
        }
    }
}