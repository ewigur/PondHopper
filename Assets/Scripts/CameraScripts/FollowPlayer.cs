using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private float offsetX = 0.2f;
    [SerializeField] private float offsetY = 0.2f;
    [SerializeField] private float damping = 0.5f;

    [SerializeField] private Transform target;
    
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    private float camHalfWidth, camHalfHeight;
    private Vector3 camVelocity;
    private float originalY;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        UpdateCameraBounds();
    }

    private void LateUpdate()
    {
        if (target == null)
            return;
        
        float targetX = target.position.x + offsetX;
        float targetY = transform.position.y  + offsetY;

        Vector3 targetPos = new Vector3(targetX, targetY, -10);
        
        float clampedX = Mathf.Clamp(targetPos.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
        float clampedY = Mathf.Clamp(targetPos.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);

        Vector3 clampedPos = new Vector3(clampedX, clampedY, -10);
        
        transform.position = Vector3.SmoothDamp(transform.position, clampedPos, ref camVelocity, damping);
    }

    private void UpdateCameraBounds()
    {
        if (cam != null)
        {
            camHalfHeight = cam.orthographicSize;
            camHalfWidth = camHalfHeight * cam.aspect;
        }
    }
}