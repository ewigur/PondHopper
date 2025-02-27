using UnityEngine;
using UnityEngine.UI;

using static PlayerHealth;
public class HealthManager : MonoBehaviour
{
    public static HealthManager HMInstance;
    
    [SerializeField] private Image[] healthImage;
    
    private void OnEnable()
    {
        OnLifeLost += UpdateHealth;
    }

    private void Awake()
    {
        if (HMInstance != null && HMInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        HMInstance = this;
    }

    private void Start()
    {
        RestoreHealthUI();
    }

    public void UpdateHealth()
    {
        int savedLives = PlayerPrefs.GetInt("remainingLives", healthImage.Length);

        for (int i = 0; i < healthImage.Length; i++)
        {
            healthImage[i].gameObject.SetActive(i < savedLives);
        }
    }

    public void RestoreHealthUI()
    {
        int savedLives = PlayerPrefs.GetInt("remainingLives", healthImage.Length);

        for (int i = 0; i < healthImage.Length; i++)
        {
            healthImage[i].gameObject.SetActive(i < savedLives);
        }
    }

    private void OnDisable()
    {
        OnLifeLost -= UpdateHealth;
    }
}