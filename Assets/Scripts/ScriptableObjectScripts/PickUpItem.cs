using UnityEngine;

[CreateAssetMenu(fileName = "PickUp", menuName = "ScriptableObjects/PickUp Item", order = 1)]
public class PickUpItem : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
    public int value;
    public int spawnAmount;
    public float flockMovement;
    public Animator pickUpAnimator;
    
    [Range(0f, 1f)]
    public float spawnProbability;
}
