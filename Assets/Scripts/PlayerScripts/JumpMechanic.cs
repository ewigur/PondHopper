using System;
using UnityEngine;
public class JumpMechanic : MonoBehaviour
{
    public static Action OnJump;

    [Header("Jump Settings")]
    [Range(0.5f, 10)]
    [SerializeField] private float minJumpForce = 5f;
    [Range(11, 30)]
    [SerializeField] private float maxJumpForce = 15f;
    
    [Header("Gravity Settings")]
    [SerializeField] private float upGravityMultiplier = 1.2f;
    [SerializeField] private float downGravityMultiplier = 3f;
    [SerializeField] private float jumpGravityMultiplier = 0.5f;
    
    [Header("Line drag Settings")]
    [SerializeField] private float maxJumpInput = 15f;

    private Rigidbody2D frogRigidBody;
    private Camera mCamera;

    private Vector2 dragStartPos;
    private LineDrawer lineDrawer;
    
    private bool isDragging;
    
    void Awake()
    {
        mCamera = Camera.main;
        frogRigidBody = GetComponent<Rigidbody2D>();
        lineDrawer = GetComponentInChildren<LineDrawer>();
    }

    void Update()
    {
        HandleInput();
        AdjustGravity();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 
            && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            StartDrag();
        }
        else if ((Input.GetMouseButton(0)) || (Input.touchCount > 0 
               && Input.GetTouch(0).phase == TouchPhase.Stationary))
        {
            UpdateDrag();
        }
        else if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 
              && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            ReleaseDrag();
        }
    }

    private void AdjustGravity()
    {
        if (frogRigidBody.linearVelocity.y > 0)
        {
            frogRigidBody.gravityScale = upGravityMultiplier;
        }
        
        else if (frogRigidBody.linearVelocity.y < 0)
        {
            frogRigidBody.gravityScale = downGravityMultiplier;
        }

        else
        {
            frogRigidBody.gravityScale = 1f;
        }
    }

    private void StartDrag()
    {
        isDragging = true;
        dragStartPos = mCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void UpdateDrag()
    {
        if (isDragging)
        {
            Vector2 endPos = mCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dragVector = endPos - dragStartPos;

            dragVector = Vector2.ClampMagnitude(dragVector, maxJumpInput);
            
            float jumpStrength = Mathf.Lerp(minJumpForce, maxJumpForce, dragVector.magnitude / maxJumpInput);
            
            lineDrawer.DrawLine(dragVector, jumpStrength);
        }
    }

    private void ReleaseDrag()
    {
        isDragging = false;
        
        Vector2 dragVector = (Vector2)mCamera.ScreenToWorldPoint(Input.mousePosition) - dragStartPos;
        dragVector = Vector2.ClampMagnitude(dragVector, maxJumpInput);
        
        PerformJump(dragVector);
        lineDrawer.ClearLine();
    }

    private void PerformJump(Vector2 dragVector)
    {
        
            float jumpStrength = Mathf.Lerp(minJumpForce, maxJumpForce, dragVector.magnitude / maxJumpInput);
            Vector2 jumpForce = dragVector.normalized * jumpStrength;

            if (IsGrounded())
            {
                jumpForce.y *= upGravityMultiplier;
                jumpForce.x *= upGravityMultiplier;
                
                frogRigidBody.linearVelocity = jumpForce;
            }

            else
            {
                jumpForce.x *= upGravityMultiplier * jumpGravityMultiplier;
                jumpForce.y *= upGravityMultiplier * jumpGravityMultiplier;
                
                frogRigidBody.linearVelocity = jumpForce;
            }
            
            OnJump?.Invoke();
    }

    private bool IsGrounded()
    {
        return Mathf.Abs(frogRigidBody.linearVelocity.y) < 0.01f;
    }
}