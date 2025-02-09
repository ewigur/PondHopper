using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [HideInInspector] public int maxHealth = 100;
    [HideInInspector] public int damageTaken = 10;
    
    public static int currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
}
