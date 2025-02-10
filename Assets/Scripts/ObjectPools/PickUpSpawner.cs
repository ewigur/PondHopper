using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private List<PickUpItem> pickUpItems;
    [SerializeField] private int defaultCapacity = 5;
    [SerializeField] private int maxActivePickUps = 10;
    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float startSpawnTime = 0.2f;

    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;
    [SerializeField] private float minY = 2f;
    [SerializeField] private float maxY = 4f;

    private int currentActivePickUps;
    
    private Dictionary<PickUpItem, ObjectPool<PickUpBehaviour>> pickUpPools;

    private void Start()
    {
        pickUpPools = new Dictionary<PickUpItem, ObjectPool<PickUpBehaviour>>();
        InitializePickUpPools();
        InvokeRepeating(nameof(Spawn), startSpawnTime, timeBetweenSpawns);
    }

    private void InitializePickUpPools()
    {
        foreach (var item in pickUpItems)
        {
            pickUpPools[item] = CreatePoolForItem(item);

            // Preload objects into the pool
            for (int i = 0; i < defaultCapacity; i++)
            {
                var pickUp = pickUpPools[item].Get();
                pickUpPools[item].Release(pickUp);
            }
        }
    }

    private ObjectPool<PickUpBehaviour> CreatePoolForItem(PickUpItem item)
    {
        return new ObjectPool<PickUpBehaviour>
        (
            createFunc: () =>
            {
                var instance = Instantiate(item.prefab).GetComponent<PickUpBehaviour>();
                instance.gameObject.SetActive(false);
                return instance;
            },
            actionOnGet: pickUp => pickUp.gameObject.SetActive(true),
            actionOnRelease: pickUp => pickUp.gameObject.SetActive(false),
            actionOnDestroy: pickUp => Destroy(pickUp.gameObject),
            collectionCheck: false, defaultCapacity, maxActivePickUps
        );
    }

    private void Spawn()
    {
        if (currentActivePickUps >= maxActivePickUps) 
            return;

        var randomPickUpItem = GetRandomPickUpItem();

        for (var i = 0; i < randomPickUpItem.spawnAmount; i++)
        {
            var pickUp = pickUpPools[randomPickUpItem].Get();
            currentActivePickUps++;

            pickUp.transform.position = GetRandomSpawnPosition();
            pickUp.Initialize(randomPickUpItem);
            pickUp.OnReturn += DisablePrefab;
        }
        
        Debug.Log("Spawned: " + randomPickUpItem.name);
    }
    
    private Vector2 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector2 spawnPosition = new Vector2(randomX, randomY);
        
        return spawnPosition;
    }

    private PickUpItem GetRandomPickUpItem()
    {
        float totalWeight = pickUpItems.Sum(item => item.spawnProbability);
        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var item in pickUpItems)
        {
            cumulativeWeight += item.spawnProbability;
            if (randomValue <= cumulativeWeight)
                return item;
        }

        return pickUpItems[0];
    }

    private void DisablePrefab(PickUpBehaviour pickUp)
    {
        if (pickUpPools.TryGetValue(pickUp.GetItemData(), out var pool))
        {
            currentActivePickUps--;
            pool.Release(pickUp);
            pickUp.OnReturn -= DisablePrefab;
        }
    }
}
