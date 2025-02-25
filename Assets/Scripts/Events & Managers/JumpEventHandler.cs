using System;
using UnityEngine;
/*
 * TODO: JumpParticles
 * TODO: JumpAnimations
 */
public class JumpEventHandler : MonoBehaviour
{
    [SerializeField] private Animator frogAnimator;

    private void Start()
    {
        FrogIdle();
    }

    private void OnEnable()
    {
        PlayerCollision.OnPlatform += FrogIdle;
        JumpMechanic.OnPreJump += PreJump;
        JumpMechanic.OnJump += JumpEvent;
    }
    
    private void FrogIdle()
    {
        if (frogAnimator == null)
            return;

        frogAnimator.Play("Idle");
    }

    private void PreJump()
    {
        frogAnimator.Play("PreJump");
    }
    
    private void JumpEvent()
    {
        frogAnimator.Play("InAir");
    }
    
    private void OnDisable()
    {
        JumpMechanic.OnPreJump -= PreJump;
        JumpMechanic.OnJump -= JumpEvent;
    }
}
