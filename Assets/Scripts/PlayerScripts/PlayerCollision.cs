using System;
using UnityEngine;
using NaughtyAttributes;

using static PlayerHealth;
using static InGameStatesHandler;
public class PlayerCollision : MonoBehaviour
{  
    public static Action<PickUpItem> OnScoreCollected;
    public static Action TriggerSpecialPickUpSound;
    public static Action TriggerPickUpSound;
    public static Action OnPlatform;

    private void OnCollisionEnter2D(Collision2D other)
    {
        PickUpBehaviour pickUpBehaviour = other.gameObject.GetComponent<PickUpBehaviour>();
        
        if (other.gameObject.CompareTag("PickUp") && pickUpBehaviour != null)
        {
            TriggerPickUpSound?.Invoke();
            AddingScore(pickUpBehaviour.itemData);
        }

        if (other.gameObject.CompareTag($"FireFly") && pickUpBehaviour != null)
        {
            TriggerSpecialPickUpSound?.Invoke();
            AddingScore(pickUpBehaviour.itemData);
        }


        if (other.gameObject.CompareTag("DeathGround"))
        {
            if (remainingLives > 0)
            {
                PHInstance.TakeDamage();
            }

            if (remainingLives <= 0)
            {
                PHInstance.PlayerDies();
            }
        }
        
        if (other.gameObject.CompareTag("Ground"))
        {
            OnPlatform?.Invoke();
        }
    }

    private void AddingScore(PickUpItem item)
    {
        OnScoreCollected?.Invoke(item);
    }
}
