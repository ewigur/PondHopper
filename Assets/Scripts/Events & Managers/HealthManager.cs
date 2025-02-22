using UnityEngine;
using UnityEngine.UI;

// TODO: Add death effects
// TODO: Add Event for Game Over (Implement real death)

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    
    private void OnEnable()
    {
        PlayerCollision.OnLifeLost+= UpdateHealthBar;
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
        Debug.Log("YOU DIED!");
        //Do cool death stuff
        //Trigger game over -screen (Action?)
    }
    
    private void OnDisable()
    {
        PlayerCollision.OnLifeLost -= UpdateHealthBar;
        PlayerCollision.OnPlayerDeath -= PlayerDeath;
    }
}
