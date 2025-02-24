using UnityEngine;
using Random = UnityEngine.Random;

public class LogBehaviour : MonoBehaviour
{
    [SerializeField] private float minStartPos;
    [SerializeField] private float maxStartPos;
    [SerializeField] private float speed;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private LogItem logItemData;
    private LogSpawner logSpawner;
    private readonly float minBounds = -15f;
    
    private float speedIncreaseInterval = 5f;
    private float speedIncrease = 0.5f;
    private float speedTimer;
    
    public void Initialize(LogItem data)
    {
        logItemData = data;
        
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        logSpawner = FindFirstObjectByType<LogSpawner>();
        
        float spawnPositionY = Random.Range(minStartPos, maxStartPos);
        rb.position = new Vector2(rb.position.x, spawnPositionY);
        
        //speed = logItemData.logSpeed;

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
        IncreaseSpeedOverTime();
    }

    private void IncreaseSpeedOverTime()
    {
        speedTimer += Time.deltaTime;
        speed = logItemData.logSpeed;
        
        if (speedTimer >= speedIncreaseInterval && speed < logItemData.absoluteMaxSpeed)
        {
            speedTimer = 0;
            
            speed += speedIncrease;
            //speed = Mathf.Min(speed, logItemData.absoluteMaxSpeed);
            
            Debug.Log(speed);
        }
        
        rb.linearVelocity = Vector2.left * speed;
    }

    private void CheckBounds()
    {
        if (transform.position.x <= minBounds)
        {
            logSpawner.ReturnLogToPool(gameObject, logItemData);
        }
    }
}
