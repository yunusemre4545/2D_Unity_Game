using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Ateþ Ayarlarý")]
    public float fireRate = 1f; 
    private float nextFireTime;
    public float minFireRate = 0.08f;

    [Header("Buff & Limit Deðiþkenleri")]
    public float bulletSpeed = 7f;
    public int bulletCount = 1;
    public int maxBulletCount = 10; 

    [Header("Roket Ayarlarý")]
    public GameObject rocketPrefab;
    private float lastTapTime = 0f;
    public float doubleTapThreshold = 0.3f; 

    void Update()
    {
        // Normal otomatik ateþ etme
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        // Çift týklama algýlama
        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastTap = Time.time - lastTapTime;
            if (timeSinceLastTap <= doubleTapThreshold)
            {
                FireRocket();
            }
            lastTapTime = Time.time;
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                float xOffset = (i - (bulletCount - 1) / 2f) * 0.3f;
                Vector3 spawnPosition = firePoint.position + new Vector3(xOffset, 0, 0);

                GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.up * bulletSpeed;
                }
            }
        }
    }

    void FireRocket()
    {
        if (GameplayManager.Instance != null && GameplayManager.Instance.rocketCount > 0)
        {
            GameplayManager.Instance.UseRocket();
            Debug.Log("Roket Fýrlatýldý! Kalan Roket: " + GameplayManager.Instance.rocketCount);

            GameObject prefabToUse = rocketPrefab != null ? rocketPrefab : bulletPrefab;

            if (prefabToUse != null && firePoint != null)
            {
                GameObject rocket = Instantiate(prefabToUse, firePoint.position, Quaternion.identity);

                if (rocketPrefab == null)
                {
                    rocket.transform.localScale = new Vector3(1.3f, 1.3f, 1f);
                }

                Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.up * (bulletSpeed * 1.5f);
                }
            }
        }
        else
        {
            Debug.Log("Yeterli Roket Yok!");
        }
    }

    // Atýþ Hýzý Buff'ý
    public void IncreaseSpeed()
    {
        fireRate -= 0.15f;

        
        if (fireRate < minFireRate)
        {
            fireRate = minFireRate;
        }

        Debug.Log("Ateþ hýzý artýrýldý! Yeni Fire Rate: " + fireRate);
    }

    // Top Sayýsý Buff'ý (ExtraBall ganimeti için)
    public void IncreaseCount()
    {
        if (bulletCount < maxBulletCount)
        {
            bulletCount++;
            Debug.Log("Top sayýsý artýrýldý! Yeni sayý: " + bulletCount);
        }
        else
        {
            Debug.Log("Maksimum top sayýsýna (Limit) ulaþýldý!");
        }
    }

    // Roket Sayýsý Buff'ý (ExtraRocket ganimeti için)
    public void AddRocket(int amount)
    {
        if (GameplayManager.Instance != null)
        {
            GameplayManager.Instance.rocketCount += amount;
            
            Debug.Log("Roket eklendi! Toplam Roket: " + GameplayManager.Instance.rocketCount);
        }
    }
}