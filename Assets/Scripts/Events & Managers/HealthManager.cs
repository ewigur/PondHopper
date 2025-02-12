using System;
using UnityEngine;
using UnityEngine.UI;

// TODO: Add death effects
// TODO: Add Event for Game Over (Implement real death)

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    
    public static Action OnDeath;
    private void OnEnable()
    {
        PlayerCollision.OnPlayerHit+= UpdateHealthBar;
        PlayerCollision.OnPlayerDeath += PlayerDeath;
    }
    
    private void UpdateHealthBar()
    {
        if (healthBar.value > 0)
        {
            healthBar.value = PlayerHealth.currentHealth;
            Debug.Log("Health Event triggered");
        }

        if (healthBar.value <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        
        OnDeath?.Invoke();
        //Do cool death stuff
        //Trigger game over -screen (Action?)
    }
    
    private void OnDisable()
    {
        PlayerCollision.OnPlayerHit -= UpdateHealthBar;
        PlayerCollision.OnPlayerDeath -= PlayerDeath;
    }
}
