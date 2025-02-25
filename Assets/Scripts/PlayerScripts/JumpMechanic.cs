using System;
using UnityEngine;
using UnityEngine.EventSystems;

using static GameManager;
public class JumpMechanic : MonoBehaviour
{
    public static Action OnPreJump;
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
    
    [Header("How much you can drag (with Input)")]
    [SerializeField] private float maxJumpInput = 15f;

    private Rigidbody2D frogRigidBody;
    private Camera mCamera;

    private Vector2 dragStartPos;
    
    private bool isDragging;
    private bool isGrounded;
    private bool canDoubleJump;
    public static bool canReceiveInput;
    
    private void Start()
    {
        mCamera = Camera.main;
        frogRigidBody = GetComponent<Rigidbody2D>();
        ToggleInput(canReceiveInput);
        canReceiveInput = true;
    }

    private void OnEnable()
    {
        onToggleInput += ToggleInput;
    }
    

    private void ToggleInput(bool isOn)
    {
        canReceiveInput = isOn;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() || !canReceiveInput)
            return;

        HandleInput();
    }
    private void FixedUpdate()
    {
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
            UpdateDrag(dragStartPos);
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
    
        OnPreJump?.Invoke();
    }

    private void UpdateDrag(Vector2 vector)
    {
        if (isDragging)
        {
            Vector2 endPos = mCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dragVector = endPos - dragStartPos;

            dragVector = Vector2.ClampMagnitude(dragVector, maxJumpInput);
            
            float jumpStrength = Mathf.Lerp(minJumpForce, maxJumpForce, dragVector.magnitude / maxJumpInput);
            
            Vector2.ClampMagnitude(vector, jumpStrength);
        }
    }

    private void ReleaseDrag()
    {
            isDragging = false;
        
            Vector2 dragVector = (Vector2)mCamera.ScreenToWorldPoint(Input.mousePosition) - dragStartPos;
            dragVector = Vector2.ClampMagnitude(dragVector, maxJumpInput);
        
            PerformJump(dragVector);
    }

    private void PerformJump(Vector2 dragVector)
    {
        float jumpStrength = Mathf.Lerp(minJumpForce, maxJumpForce, dragVector.magnitude / maxJumpInput);
        Vector2 jumpForce = dragVector.normalized * jumpStrength;

        if (IsGrounded())
        {
            canDoubleJump = true;
            jumpForce.y *= upGravityMultiplier;
            jumpForce.x *= upGravityMultiplier;
            
            frogRigidBody.linearVelocity = jumpForce;
            OnJump?.Invoke();
        }

        else if(canDoubleJump)
        {
            canDoubleJump = false;
            jumpForce.x *= upGravityMultiplier * jumpGravityMultiplier;
            jumpForce.y *= upGravityMultiplier * jumpGravityMultiplier;
            
            frogRigidBody.linearVelocity = jumpForce;
            OnJump?.Invoke();
        }
        
    }
    
    private bool IsGrounded()
    {
        isGrounded = Mathf.Abs(frogRigidBody.linearVelocity.y) < 0.01f;
        
        if (isGrounded)
        {
            canDoubleJump = true;
        }
        
        return isGrounded;
    }

    private void OnDisable()
    {
        onToggleInput -= ToggleInput;
    }
}