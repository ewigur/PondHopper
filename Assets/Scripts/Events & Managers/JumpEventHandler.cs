using UnityEngine;

public class JumpEventHandler : MonoBehaviour
{
    [SerializeField] private Animator frogAnimator;
    private void OnEnable()
    {
        PlayerCollision.OnPlatform += FrogIdle;
        JumpMechanic.OnPreJump += PreJump;
        JumpMechanic.OnJump += JumpEvent;
    }
    
    private void FrogIdle()
    {
        frogAnimator.Play("Idle");
        Debug.Log("Frog Idle");
    }

    private void PreJump()
    {
        frogAnimator.Play("PreJump");
        Debug.Log("Waiting for jump action");
    }
    
    private void JumpEvent()
    {
        frogAnimator.Play("InAir");
        Debug.Log("Jump action executed");
        /*
         * TODO: JumpParticles
         * TODO: JumpAnimations
         */
    }
    
    private void OnDisable()
    {
        JumpMechanic.OnPreJump -= PreJump;
        JumpMechanic.OnJump -= JumpEvent;
    }
}
