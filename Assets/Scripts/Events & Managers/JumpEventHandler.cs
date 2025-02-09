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
        Debug.Log("JumpEvent triggered");
    }
    
    private void OnDisable()
    {
        JumpMechanic.OnJump -= JumpEvent;
    }
}
