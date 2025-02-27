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
            PlayerPrefs.SetInt("remainingLives", remainingLives);
        }
        else if (GMInstance.state == GameStates.GameRestarted)
        {
            remainingLives = PlayerPrefs.GetInt("remainingLives", maxLives);
        }
    }
    
    public void TakeDamage()
    {
        remainingLives -= damageTaken;
        
        if (remainingLives > 0)
        {
            PlayerPrefs.SetInt("remainingLives", remainingLives);
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
        }
    }
}