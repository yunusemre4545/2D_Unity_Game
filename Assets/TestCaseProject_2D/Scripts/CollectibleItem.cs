using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    // Ganimet türlerini belirliyoruz
    public enum ItemType { ExtraHealth, FireRateBoost, ExtraRocket, ExtraBall }
    public ItemType currentItemType;

    public float moveSpeed = 4f; // Oyuncuya doðru süzülme hýzý

    void Update()
    {
        // Ganimet yavaþça aþaðýya doðru süzülsün
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        // Ekranýn çok altýna indiyse yok ol (Performans için)
        if (transform.position.y < -7f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Eðer çarpan þey Player ise
        if (other.CompareTag("Player") || other.name.Contains("Player"))
        {
            ApplyEffect();
            Destroy(gameObject); // Ganimeti toplandýktan sonra yok et
        }
    }

    [Header("Bildirim Prefabý")]
    public GameObject popupTextPrefab; // Hazýrladýðýmýz UI prefabý

    void ApplyEffect()
    {
        if (GameplayManager.Instance != null)
        {
            string message = "";
            Color msgColor = Color.white;

            PlayerShooting playerShooting = FindObjectOfType<PlayerShooting>();

            switch (currentItemType)
            {
                case ItemType.ExtraHealth:
                    GameplayManager.Instance.playerHealth++;
                    if (GameplayManager.Instance.healthText != null)
                        GameplayManager.Instance.healthText.text = "HP : " + GameplayManager.Instance.playerHealth.ToString();
                    message = "+1 CAN!";
                    msgColor = Color.green;
                    break;

                case ItemType.FireRateBoost:
                    if (playerShooting != null) playerShooting.IncreaseSpeed();
                    message = "ATIÞ HIZI ARTTI!";
                    msgColor = Color.yellow;
                    break;

                case ItemType.ExtraRocket:
                    if (playerShooting != null) playerShooting.AddRocket(5); // +5 Roket ekler
                    message = "+5 ROKET!";
                    msgColor = Color.red;
                    break;

                case ItemType.ExtraBall:
                    if (playerShooting != null) playerShooting.IncreaseCount();
                    message = "+1 TOP!";
                    msgColor = Color.cyan;
                    break;
            }

            // Yazýyý ana Canvas altýnda yaratýp ekranda gösterelim
            if (popupTextPrefab != null)
            {
                Canvas canvas = FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    GameObject popup = Instantiate(popupTextPrefab, canvas.transform);

                    RectTransform rectTransform = popup.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0.5f, 0));
                        rectTransform.position = screenPoint;
                    }

                    FloatingText floatScript = popup.GetComponent<FloatingText>();
                    if (floatScript != null)
                    {
                        floatScript.SetText(message, msgColor);
                    }
                }
            }
        }
    }
}