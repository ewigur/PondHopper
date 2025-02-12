using UnityEngine;
/*
 * TODO: JumpParticles
 * TODO: JumpSounds
 * TODO: JumpAnimations
 */
public class JumpEventHandler : MonoBehaviour
{
    private void OnEnable()
    {
        JumpMechanic.OnJump += JumpEvent;
    }
    
    private void JumpEvent()
    {
        
    }
    
    private void OnDisable()
    {
        JumpMechanic.OnJump -= JumpEvent;
    }
}
