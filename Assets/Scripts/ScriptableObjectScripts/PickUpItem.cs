using UnityEngine;

[CreateAssetMenu(fileName = "PickUp", menuName = "ScriptableObjects/PickUp Item", order = 1)]
public class PickUpItem : ScriptableObject
{
    public string itemName;
    
    public Animator pickUpAnimator;
    public float flockMovement;
    public GameObject prefab;
    public int spawnAmount;
    public int value;
    
    [Range(0f, 1f)]
    public float spawnProbability;
}
