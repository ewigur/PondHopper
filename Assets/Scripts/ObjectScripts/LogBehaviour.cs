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
    private LogPool logPool;
    private readonly float minBounds = -15f;
    
    private readonly float speedIncreaseInterval = 20f;
    private readonly float speedIncrease = 0.25f;
    private float speedTimer;
    private void Start()
    {
        speed = logItemData.logSpeed;
    }

    public void Initialize(LogItem data)
    {
        logItemData = data;
        
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        logPool = FindFirstObjectByType<LogPool>();

        float[] spawnVariations = { minStartPos, maxStartPos };
        var randomVariation = Random.Range(0, spawnVariations.Length);
        
        var spawnPositionY = spawnVariations[randomVariation];
        
        rb.position = new Vector2(rb.position.x, spawnPositionY);

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
        float depthIllusionBackground = -0.7f;
        float depthIllusionForeground = -1f;
        
        Vector2 smallScaleBG = new Vector2(0.9f, 0.9f);
        Vector2 bigScaleBG = new Vector2(1.1f, 1.1f);
        
        Vector2 ScaleFG = new Vector2(1.2f, 1.2f);
    
        if (yPos >= depthIllusionBackground) 
        {
            transform.localScale = smallScaleBG;
        }
        else if (yPos < depthIllusionBackground && yPos > depthIllusionForeground) 
        {
            transform.localScale = bigScaleBG;
        }

        else //if(yPos <= depthIllusionForeground)
        {
            transform.localScale = ScaleFG;
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
        
        if (speedTimer >= speedIncreaseInterval && speed < logItemData.absoluteMaxSpeed)
        {
            speedTimer = 0;
            
            speed += speedIncrease;
        }
        
        rb.linearVelocity = Vector2.left * speed;
    }

    private void CheckBounds()
    {
        if (transform.position.x <= minBounds)
        {
            logPool.ReturnLogToPool(gameObject, logItemData);
        }
    }
}
