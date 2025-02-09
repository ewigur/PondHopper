using UnityEngine;

public class ProjectileDropperScript : MonoBehaviour
{
    public GameObject projectile;

    private float minTime = 3f;
    private float maxTime = 10f;
    private float timeBetweenProjectiles;
    private float lastProjectileDropTime = 0f;
    void Start()
    {
        SetRandomTime();
        lastProjectileDropTime = Time.time;
    }

    private void SetRandomTime()
    {
        timeBetweenProjectiles = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        DropProjectile();
    }

    private void DropProjectile()
    {
        if (Time.time - lastProjectileDropTime >= timeBetweenProjectiles)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            
            lastProjectileDropTime = Time.time;
            SetRandomTime();
        }
    }
}
