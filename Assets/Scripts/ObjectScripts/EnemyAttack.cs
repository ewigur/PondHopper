using UnityEngine;
using System;

//TODO: Add movement
//TODO: Maybe use raycast instead of range??

public class EnemyAttack : MonoBehaviour
{
    public Transform playerTransform;
    
    private readonly float detectionRange = 2f;

    public static Action OnAttack;

    private void Start()
    {
        
    }

    void Update()
    {
        AttackPattern();
    }
    public void AttackPattern()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= detectionRange)
        {
            
        }
    }
}
