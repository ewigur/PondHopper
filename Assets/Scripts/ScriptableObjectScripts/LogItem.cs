using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LogItem", order = 2)]
public class LogItem : ScriptableObject
{
    public string typeOfLog;
    
    public float absoluteMaxSpeed = 5f;
    public GameObject prefab;
    public float logSpeed;
    
    [Range(0, 1)]
    public float spawnPercent;
}
