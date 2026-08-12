using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Slider healthBar;

    [Header("Giriþ Animasyonu")]
    public float descentSpeed = 2f;
    public Vector3 targetPosition = new Vector3(0f, 3.5f, 0f);
    private bool isDescending = true;
    private Collider2D bossCollider;

    [Header("Saldýrý Ayarlarý (3 Farklý Nokta)")]
    public GameObject bossBulletPrefab;
    public Transform[] firePoints; // 3 farklý ateþ noktasý için dizi
    public float minFireRate = 0.8f; // En kýsa ateþ etme aralýðý
    public float maxFireRate = 2.0f; // En uzun ateþ etme aralýðý
    public float bulletSpeed = 5f;

    private float[] nextFireTimes; // Her bir ateþ noktasýnýn kendi baðýmsýz zaman sayacý

    public void InitializeBoss(Slider uiHealthBar)
    {
        currentHealth = maxHealth;
        healthBar = uiHealthBar;

        healthBar.gameObject.SetActive(true);
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        bossCollider = GetComponent<Collider2D>();
        if (bossCollider != null)
        {
            bossCollider.enabled = false;
        }

        // Ateþ noktalarý sayýsý kadar zaman sayacý dizisini baþlatýyoruz
        if (firePoints != null && firePoints.Length > 0)
        {
            nextFireTimes = new float[firePoints.Length];
            for (int i = 0; i < firePoints.Length; i++)
            {
                nextFireTimes[i] = Time.time + Random.Range(minFireRate, maxFireRate);
            }
        }
    }

    void Update()
    {
        if (isDescending)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, descentSpeed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                isDescending = false;
                Debug.Log("Boss yerine ulaþtý! Savaþ ve Baðýmsýz Atýþlar baþlýyor.");

                if (bossCollider != null)
                {
                    bossCollider.enabled = true;
                }

                Spawner spawner = FindFirstObjectByType<Spawner>();
                if (spawner != null)
                {
                    spawner.enabled = true;
                }
            }
        }
        else
        {
            // 3 farklý ateþ noktasýný baðýmsýz ve rastgele sürelerle kontrol ediyoruz
            if (firePoints != null && nextFireTimes != null)
            {
                for (int i = 0; i < firePoints.Length; i++)
                {
                    if (Time.time >= nextFireTimes[i])
                    {
                        ShootFromPoint(firePoints[i]);
                        // Her atýþtan sonra bu noktaya özel yeni bir rastgele süre belirliyoruz
                        nextFireTimes[i] = Time.time + Random.Range(minFireRate, maxFireRate);
                    }
                }
            }
        }
    }

    void ShootFromPoint(Transform pointToShoot)
    {
        if (bossBulletPrefab != null && pointToShoot != null)
        {
            GameObject bullet = Instantiate(bossBulletPrefab, pointToShoot.position, Quaternion.identity);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.down * bulletSpeed;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDescending) return;

        if (other.name.Contains("Boss")) return;

        if (other.name.Contains("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDescending) return;

        currentHealth -= damageAmount;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss Öldü! Oyunu Kazandýn!");
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }

        SceneManager.LoadScene("CompleteScene");
        Destroy(gameObject);
    }
}