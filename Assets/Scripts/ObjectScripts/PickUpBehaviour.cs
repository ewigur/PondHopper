using UnityEngine;
using Random = UnityEngine.Random;

//TODO: Add better movement in FixedUpdate when I have a clearer view of what animations I should have
//TODO: Code Cleanup
public delegate void Return(PickUpBehaviour pickUp);
public class PickUpBehaviour : MonoBehaviour
{
    [HideInInspector]
    public PickUpItem itemData;
    private readonly float screenBoundsMargin = 1f; 
    
    private Rigidbody2D rb;
    private Camera cam;
    
    public Return OnReturn;
    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(PickUpItem data)
    {
        itemData = data;
    }

    public PickUpItem GetItemData()
    {
        return itemData;
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            Vector2 randomMovement = Random.insideUnitCircle * (itemData.flockMovement * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + randomMovement);
        }
        
        CheckOutOfBounds();
    }

    private void CheckOutOfBounds()
    {
        Vector2 screenPosition = cam.WorldToViewportPoint(transform.position);

        if (screenPosition.x < -screenBoundsMargin || screenPosition.x > screenBoundsMargin ||
            screenPosition.y < -screenBoundsMargin || screenPosition.y > screenBoundsMargin)
        {
            OnReturn?.Invoke(this);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Collided with: " + collision.transform.name);
            OnReturn?.Invoke(this);
        }
    }
}