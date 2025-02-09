using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float minSpeed = 5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private BoxCollider2D directionCollider;

    private float randomSpeed;
    
    private Rigidbody2D enemyRb;
    private readonly float speedMultiplier = 10f;
    private void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        EnemyMoving();
    }

    private void EnemyMoving()
    {
        randomSpeed = Random.Range(minSpeed, maxSpeed);
        
        if(IsFacingRight())
        {
            enemyRb.linearVelocity = new Vector2(randomSpeed * speedMultiplier * Time.fixedDeltaTime, 0);
        }
        
        else
        {
            enemyRb.linearVelocity = new Vector2(-randomSpeed * speedMultiplier * Time.fixedDeltaTime, 0);
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-Mathf.Sign(enemyRb.linearVelocity.x), transform.localScale.y);
    }
}

