using UnityEngine;
using System;
using NaughtyAttributes;

// TODO: Add list of Lives instead of health bar, deathground triggers lives lost
public class PlayerCollision : MonoBehaviour
{  
    public static Action<PickUpItem> OnScoreCollected;
    public static Action TriggerPickUpSound;
    public static Action OnLifeLost;
    public static Action OnPlayerDeath;
    
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PickUpBehaviour pickUpBehaviour = other.gameObject.GetComponent<PickUpBehaviour>();
        
        if (other.gameObject.CompareTag("PickUp") && pickUpBehaviour != null)
        {
            TriggerPickUpSound?.Invoke();
            AddingScore(pickUpBehaviour.itemData);
        }

        if (other.gameObject.CompareTag("Enemy") && playerHealth != null)
        {
            TakeDamage(playerHealth.damageTaken);
        }

        if (other.gameObject.CompareTag("DeathGround") && playerHealth != null)
        {
            OnPlayerDeath?.Invoke();
        }
    }

    private void AddingScore(PickUpItem item)
    {
        OnScoreCollected?.Invoke(item);
    }
    
    [Button]
    public void TestDamageTaken()
    {
        TakeDamage(playerHealth.damageTaken);
    }
    
    public void TakeDamage(int damage)
    {
        PlayerHealth.currentHealth -= damage;
        OnLifeLost.Invoke();
        
        Debug.Log("Damage Taken: " + playerHealth.damageTaken+ "Current Health: " + PlayerHealth.currentHealth);
    }
}
