using System;
using UnityEngine;

// TODO: add animation to projectile 
public class ProjectileBehaviour : MonoBehaviour
{
    private Rigidbody2D projectileRb;

    private void Start()
    {
        projectileRb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("DeathGround"))
        {
            Destroy(gameObject, 0.3f);
            
        }
    }
}
