using UnityEngine;
using Random = UnityEngine.Random;

public class LogBehaviour : MonoBehaviour
{
    [SerializeField] private float minStartPos;
    [SerializeField] private float maxStartPos;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private LogItem logItemData;
    private LogSpawner logSpawner;
    private readonly float minBounds = -15f;
    
    public void Initialize(LogItem data)
    {
        logItemData = data;
        
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        logSpawner = FindFirstObjectByType<LogSpawner>();
        
        float spawnPositionY = Random.Range(minStartPos, maxStartPos);
        rb.position = new Vector2(rb.position.x, spawnPositionY);
        
        float randomSpeed = Random.Range(logItemData.MinLogSpeed, logItemData.MaxLogSpeed);
        rb.linearVelocity = Vector2.left * randomSpeed;

        SetScale(spawnPositionY);
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

    private void SetScale(float yPos)
    {
        Vector2 depthIllusion = new Vector2(0.9f, 0.9f);

        if (yPos <= minStartPos)
        {
            transform.localScale *= depthIllusion;
        }
    }

    private void Update()
    {
        CheckBounds();
    }

    private void CheckBounds()
    {
        if (transform.position.x <= minBounds)
        {
            logSpawner.ReturnLogToPool(gameObject, logItemData);
        }
    }
}
