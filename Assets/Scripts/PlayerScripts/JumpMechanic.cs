using System;
using System.Collections.Generic;
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
    [SerializeField] private float DJGravityMultiplier = 0.5f;
    
    [Header("How much you can drag (with Input)")]
    [SerializeField] private float maxJumpInput = 15f;

    private Rigidbody2D frogRigidBody;
    private LineDrawer lineDrawer;
    private Camera mCamera;

    private Vector2 dragStartPos;
    private Vector2 dragVector;

    private float lineMagnitude;
    private bool isDragging;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool canReceiveInput = true;
    
    private readonly float resumeInputCooldown = 0.2f;
    private float currentLineLength;
    private float lastResumeTime;
    private const float lineLerpSpeed = 5f;

    private void Start()
    {
        ToggleInput(canReceiveInput);

        mCamera = Camera.main;
        frogRigidBody = GetComponent<Rigidbody2D>();
        lineDrawer = GetComponentInChildren<LineDrawer>();
    }

    private void OnEnable()
    {
        onToggleInput += ToggleInput;
        onGameStateChanged += HandleStateChange;
    }

    private void ToggleInput(bool isOn)
    {
        canReceiveInput = isOn;
    }
    
    private void HandleStateChange(GameStates newState)
    {
        if (newState == GameStates.GameResumed || newState == GameStates.GameLoop)
        {
            lastResumeTime = Time.unscaledTime;
        }
    }

    private bool IsPointerOverUI()
    {
        var eventData = new PointerEventData(EventSystem.current);
        {
            eventData.position = Input.mousePosition;
        }
        
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        
        return results.Count > 0;
    }

    private void Update()
    {
        if (Time.unscaledTime - lastResumeTime < resumeInputCooldown) 
            return;
        

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Began))
        {
            if (IsPointerOverUI())
                return;
        }

        HandleInput();
    }
    private void FixedUpdate()
    {
        AdjustGravity();
    }
    
    private void HandleInput()
    {
        if (GMInstance.state == GameStates.GamePaused || 
            GMInstance.state == GameStates.GameOver)
            return;
        
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && 
            Input.GetTouch(0).phase == TouchPhase.Began))
        {
            StartDrag();
        }
        else if (Input.GetMouseButton(0) || (Input.touchCount > 0 && 
                 Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            UpdateDrag();
        }
        else if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && 
                 Input.GetTouch(0).phase == TouchPhase.Ended))
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

        OnPreJump?.Invoke();
    }
    

    private void UpdateDrag()
    {
        if (isDragging && IsGrounded())
        {
            dragStartPos = transform.position;
            dragVector = mCamera.ScreenToWorldPoint(Input.mousePosition) - (Vector3)dragStartPos;
            dragVector = Vector2.ClampMagnitude(dragVector, maxJumpInput);
            
            float targetLineLength = Mathf.Lerp(minJumpForce, maxJumpForce, dragVector.magnitude / maxJumpInput);
            currentLineLength = Mathf.Lerp(currentLineLength, targetLineLength, Time.deltaTime * lineLerpSpeed);
            Vector2 displayVector = dragVector.normalized * currentLineLength;
            
            lineDrawer.DrawLine(dragStartPos, dragStartPos + displayVector);
        }
    }

    private void ReleaseDrag()
    {
        isDragging = false;
    
        dragVector = (Vector2)mCamera.ScreenToWorldPoint(Input.mousePosition) - dragStartPos;
        dragVector = Vector2.ClampMagnitude(dragVector, maxJumpInput);
    
        PerformJump(dragVector);
        
        lineDrawer.ClearLine();
    }

    private void PerformJump(Vector2 vector)
    {
        float jumpStrength = Mathf.Lerp(minJumpForce, maxJumpForce, vector.magnitude / maxJumpInput);
        Vector2 jumpForce = vector.normalized * jumpStrength;

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
            jumpForce.x *= upGravityMultiplier * DJGravityMultiplier;
            jumpForce.y *= upGravityMultiplier * DJGravityMultiplier;
            
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