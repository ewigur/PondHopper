using System;
using UnityEngine;

using static GameManager;
public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth PHInstance;
    
    public static Action PlayerHasDied;
    public static Action OnLifeLost;

    public readonly int maxLives = 3;
    private readonly int damageTaken = 1;

    public string savedHealth = "remainingLives";
    public static int remainingLives;
    
    void Awake()
    {
        if (PHInstance != null && PHInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        PHInstance = this;
    }

    private void Start()
    {
        if (GMInstance.state == GameStates.GameLoop)
        {
            remainingLives = maxLives;
            PlayerPrefs.SetInt(savedHealth, maxLives);
            PlayerPrefs.Save();
        }
        else if (GMInstance.state == GameStates.GameRestarted)
        {
            if (PlayerPrefs.HasKey(savedHealth))
            {
                remainingLives = PlayerPrefs.GetInt(savedHealth);
            }

            else
            {
                remainingLives = maxLives;
            }
        }
    }
    
    public void TakeDamage()
    {
        remainingLives -= damageTaken;
        
        if (remainingLives > 0)
        {
            PlayerPrefs.SetInt(savedHealth, remainingLives);
            PlayerPrefs.Save();
            OnLifeLost?.Invoke();
        }
        else
        {
            PlayerDies();
        }
    }

    public void PlayerDies()
    {
        if (remainingLives <= 0)
        {
            PlayerHasDied?.Invoke();
            
            PlayerPrefs.SetInt(savedHealth, maxLives);
            PlayerPrefs.Save();
        }
    }
}