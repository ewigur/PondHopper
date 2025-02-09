using UnityEngine;
using Random = UnityEngine.Random;

public class LogBehaviour : MonoBehaviour
{
    [SerializeField] private float startPosition1;
    [SerializeField] private float startPosition2;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private LogItem logItemData;
    private LogSpawner logSpawner;
    private readonly float minBounds = -30f;
    private readonly float maxBounds = 20f;
    
    public void Initialize(LogItem data)
    {
        logItemData = data;
        
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        logSpawner = FindFirstObjectByType<LogSpawner>();
        
        float spawnPositionY = Random.Range(startPosition1, startPosition2);
        rb.position = new Vector2(rb.position.x, spawnPositionY);
        
        float randomSpeed = Random.Range(logItemData.MinLogSpeed, logItemData.MaxLogSpeed);
        rb.linearVelocity = Vector2.left * randomSpeed;

        SetSortingOrder(spawnPositionY);
    }

    private void SetSortingOrder(float yPosition)
    {
        if (sr != null)
        {
            int sortingOrder = Mathf.RoundToInt(-yPosition * 10);
            sr.sortingOrder = sortingOrder;
        }
    }

    private void Update()
    {
        CheckBounds();
    }

    private void CheckBounds()
    {
        if (transform.position.x <= minBounds || transform.position.x >= maxBounds
            || transform.position.y <= minBounds || transform.position.y >= maxBounds)
        {
            Debug.Log("Log out of bounds, returning to pool");
            logSpawner.ReturnLogToPool(gameObject, logItemData);
        }
    }
}
