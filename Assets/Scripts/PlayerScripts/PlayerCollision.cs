using UnityEngine;
using System;
using NaughtyAttributes;
public class PlayerCollision : MonoBehaviour
{  
    public static Action<PickUpItem> OnScoreCollected;
    public static Action OnPlayerHit;
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
        OnPlayerHit.Invoke();
        
        Debug.Log("Damage Taken: " + playerHealth.damageTaken+ "Current Health: " + PlayerHealth.currentHealth);
    }
}
