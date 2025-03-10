using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public delegate void Return(PickUpBehaviour pickUp);
public class PickUpBehaviour : MonoBehaviour
{
    private Vector2 targetPosition;
    
    private float minMoveRangeX = -7f;
    private float maxMoveRangeX = 7f;
    private float minMoveRangeY = 0f;
    private float maxMoveRangeY = 4f;

    [HideInInspector]
    public PickUpItem itemData;
    private readonly float screenBoundsMarginY = 6f;
    
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Camera cam;
    
    public Return OnReturn;
    
    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        DirectionFlipper(targetPosition);
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
            transform.position = Vector2.MoveTowards(transform.position, targetPosition,
                    Time.fixedDeltaTime * itemData.flockMovement);

        if((Vector2)transform.position == targetPosition)
        {
            targetPosition = new Vector2(Random.Range(minMoveRangeX, maxMoveRangeX), 
                                         Random.Range(minMoveRangeY, maxMoveRangeY));
            DirectionFlipper(targetPosition);
            
            transform.position = Vector2.MoveTowards(transform.position, targetPosition,
                    Time.fixedDeltaTime * itemData.flockMovement);
        }

        CheckOutOfBounds();
    }

    private void DirectionFlipper(Vector2 direction)
    {
        rb.linearVelocity = (direction - (Vector2)transform.position).normalized * itemData.flockMovement;
        spriteRenderer.flipX = direction.x > transform.position.x;

    }

    private void CheckOutOfBounds()
    {
        if(transform.position.y < -screenBoundsMarginY)
        {
            OnReturn?.Invoke(this);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            OnReturn?.Invoke(this);
        }
    }
}