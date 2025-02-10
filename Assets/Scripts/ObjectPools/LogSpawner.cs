using System.Collections.Generic;
using UnityEngine;

public class LogSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<LogItem> logItems;
    [SerializeField] private float timeBetweenLogs = 3f;
    [SerializeField] private float minTimeBetweenLogs = 1.5f;
    [SerializeField] private float maxTimeBetweenLogs = 2f;

    [SerializeField] private int poolSizePerLog = 5;
    [SerializeField] private int maxActiveLogs = 5;

    private float totalSpawnRate;
    private int currentActiveLogs = 0;
    private Dictionary<LogItem, Queue<GameObject>> logItemPool;

    void Start()
    {
        InitializePool();
        CalculateTotalSpawnRate();
        
        InvokeRepeating(nameof(SpawnLogs),
            Random.Range(minTimeBetweenLogs, maxTimeBetweenLogs), timeBetweenLogs);
    }

    private void InitializePool()
    {
        logItemPool = new Dictionary<LogItem, Queue<GameObject>>();

        foreach (LogItem logItem in logItems)
        {
            Queue<GameObject> logQueue = new Queue<GameObject>();

            for (int i = 0; i < poolSizePerLog; i++)
            {
                GameObject log = Instantiate(logItem.prefab, spawnPoint.position, Quaternion.identity);
                log.SetActive(false);
                logQueue.Enqueue(log);
            }

            logItemPool[logItem] = logQueue;
        }
    }

    private void CalculateTotalSpawnRate()
    {
        totalSpawnRate = 0f;
        foreach (var logItem in logItems)
        {
            totalSpawnRate += logItem.spawnPercent;
        }
    }

    private void SpawnLogs()
    {
        if (currentActiveLogs >= maxActiveLogs || logItems == null || logItems.Count == 0)
            return;

        LogItem selectedLog = SelectRandomLog();
        if (selectedLog == null || !logItemPool.ContainsKey(selectedLog))
            return;

        GameObject log = GetLogFromPool(selectedLog);
        if (log == null)
            return;

        log.transform.position = spawnPoint.position;
        log.SetActive(true);

        var logBehaviour = log.GetComponent<LogBehaviour>();
        if (logBehaviour != null)
        {
            currentActiveLogs++;
            logBehaviour.Initialize(selectedLog);
        }
    }

    private LogItem SelectRandomLog()
    {
        float randomValue = Random.Range(0f, totalSpawnRate);
        float cumulativeProbability = 0f;

        foreach (var logItem in logItems)
        {
            cumulativeProbability += logItem.spawnPercent;
            if (randomValue <= cumulativeProbability)
                return logItem;
        }

        return null;
    }

    private GameObject GetLogFromPool(LogItem logType)
    {
        if (logItemPool.TryGetValue(logType, out Queue<GameObject> pool) && pool.Count > 0)
            return pool.Dequeue();
        
        return null;
    }

    public void ReturnLogToPool(GameObject log, LogItem logItem)
    {
        if (log == null || logItem == null)
            return;

        currentActiveLogs--;
        log.SetActive(false);

        if (logItemPool.TryGetValue(logItem, out Queue<GameObject> pool))
        {
            pool.Enqueue(log);
        }
    }
}