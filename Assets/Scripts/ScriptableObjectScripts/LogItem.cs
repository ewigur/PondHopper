using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/LogItem", order = 2)]
public class LogItem : ScriptableObject
{
    public string typeOfLog;
    public float MaxLogSpeed;
    public float MinLogSpeed;
    public GameObject prefab;
    
    [Range(0, 1)]
    public float spawnPercent;
}
