using UnityEngine;
using TMPro;

public class DestructibleBlock : MonoBehaviour
{
    public int blockHealth = 3;
    private TextMeshPro healthText;

    [Header("Efektler")]
    public GameObject explosionEffectPrefab;

    [Header("Ganimet Ayarlarý")]
    public GameObject collectiblePrefab; 
    [Range(0f, 100f)] public float dropChance = 10f; 

    void Awake()
    {
        healthText = GetComponentInChildren<TextMeshPro>();

        int currentLevel = 1;
        if (GameplayManager.Instance != null)
        {
            currentLevel = GameplayManager.Instance.currentDifficultyLevel;
        }

        int minHealth = 1 + (currentLevel - 1);
        int maxHealth = 4 + (currentLevel - 1);
        blockHealth = Random.Range(minHealth, maxHealth);
    }

    void Start()
    {
        if (healthText != null)
        {
            healthText.text = blockHealth.ToString();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Contains("Bullet"))
        {
            Destroy(other.gameObject);
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        blockHealth -= damageAmount;

        if (healthText != null)
        {
            healthText.text = blockHealth.ToString();
        }

        if (blockHealth <= 0)
        {
            if (GameplayManager.Instance != null)
            {
                GameplayManager.Instance.AddDestroyedBlock();
            }

            
            if (explosionEffectPrefab != null)
            {
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            }

            // Ganimet Düþürme Mantýðý
            if (collectiblePrefab != null)
            {
                float roll = Random.Range(0f, 100f);
                if (roll <= dropChance)
                {
                    GameObject droppedItem = Instantiate(collectiblePrefab, transform.position, Quaternion.identity);

                    CollectibleItem itemScript = droppedItem.GetComponent<CollectibleItem>();
                    if (itemScript != null)
                    {
                        int randomType = Random.Range(0, 4);
                        itemScript.currentItemType = (CollectibleItem.ItemType)randomType;
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}