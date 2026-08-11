using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    private Rigidbody2D rb;
    public float baseFallSpeed = 1f; // Temel hýz

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
        }

        // GameplayManager'dan mevcut level'ý çekiyoruz
        int currentLevel = 1;
        if (GameplayManager.Instance != null)
        {
            currentLevel = GameplayManager.Instance.currentDifficultyLevel;
        }

        // Her level arttýkça hýza ekleme yapýyoruz (Örn: Her level +1.2 hýz ekler)
        float finalSpeed = baseFallSpeed + ((currentLevel - 1) * 1.2f);

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0, -finalSpeed);
        }
    }

    void Update()
    {
        // Eðer havada süzülürken hýzýn sabit kalmasýný istiyorsan Start yeterlidir.
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null || other.name.Contains("Player"))
        {
            if (GameplayManager.Instance != null)
            {
                GameplayManager.Instance.TakeDamage();
            }
            Destroy(gameObject);
        }
    }
}