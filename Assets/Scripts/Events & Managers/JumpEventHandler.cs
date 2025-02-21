using UnityEngine;

public class JumpEventHandler : MonoBehaviour
{
    private void OnEnable()
    {
        JumpMechanic.OnJump += JumpEvent;
    }
    
    private void JumpEvent()
    {
        /*
         * TODO: JumpParticles
         * TODO: JumpAnimations
         */
    }
    
    private void OnDisable()
    {
        JumpMechanic.OnJump -= JumpEvent;
    }
}
