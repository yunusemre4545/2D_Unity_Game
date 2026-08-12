using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    public Slider progressBar;
    public TextMeshProUGUI levelText;

    public int playerHealth = 3;
    public TextMeshProUGUI healthText;

    private float currentProgress = 0f;
    public float maxProgress = 100f;
    public float levelDuration = 150f;
    private float timer = 0f;

    public int currentDifficultyLevel = 1;

    public bool isBuffSelectionActive = false;
    public GameObject buffSelectionPanel;
    private float selectionTimer = 0f;
    public float maxSelectionTime = 10f;

    [Header("Roket Sistemi")]
    public int rocketCount = 0;
    private int destroyedBlockCount = 0;
    public TextMeshProUGUI rocketText;

    [Header("Boss Sistemi")]
    public bool isBossPhase = false;
    public GameObject bossPrefab;
    public Slider bossHealthBar;
    public TextMeshProUGUI bossFightText; 

    // Hit-Stop Mekanizmasý
    public System.Collections.IEnumerator DoHitStop(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration); 
        Time.timeScale = 1f; 
    }
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (progressBar != null)
        {
            progressBar.maxValue = maxProgress;
            progressBar.value = 0f;
        }
        if (buffSelectionPanel != null) buffSelectionPanel.SetActive(false);
        if (bossFightText != null) bossFightText.gameObject.SetActive(false); 

        UpdateLevelText();
        UpdateHealthUI();
        UpdateRocketUI();
    }

    void Update()
    {
        if (currentProgress < maxProgress && !isBossPhase)
        {
            timer += Time.deltaTime;
            currentProgress = (timer / levelDuration) * maxProgress;
            currentProgress = Mathf.Clamp(currentProgress, 0f, maxProgress);

            if (progressBar != null) progressBar.value = currentProgress;

            float percentage = (currentProgress / maxProgress) * 100f;
            int calculatedLevel = Mathf.FloorToInt(percentage / 20f) + 1;
            if (calculatedLevel > 5) calculatedLevel = 5;

            if (calculatedLevel > currentDifficultyLevel)
            {
                currentDifficultyLevel = calculatedLevel;
                UpdateLevelText();
                TriggerBuffSelection();
            }

            if (currentProgress >= maxProgress)
            {
                StartCoroutine(StartBossPhase());
            }
        }
    }

    private IEnumerator StartBossPhase()
    {
        isBossPhase = true;
        Debug.Log("Süre doldu! Boss aþamasýna geçiliyor...");

        Spawner spawner = FindFirstObjectByType<Spawner>();
        if (spawner != null) spawner.enabled = false;

        if (levelText != null) levelText.gameObject.SetActive(false);

        
        if (bossFightText != null) bossFightText.gameObject.SetActive(true);

        Debug.Log("3 saniyelik sessizlik ve Boss Fight yazýsý ekranda...");
        yield return new WaitForSeconds(3f);

        
        if (bossFightText != null) bossFightText.gameObject.SetActive(false);

        Debug.Log("Boss Geliyor!");
        if (bossPrefab != null)
        {
            Vector3 spawnPos = new Vector3(0f, 8f, 0f);
            GameObject bossObj = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

            BossController bossCtrl = bossObj.GetComponent<BossController>();
            if (bossCtrl != null && bossHealthBar != null)
            {
                bossCtrl.InitializeBoss(bossHealthBar);
            }
        }
        else
        {
            Debug.LogError("HATA: Boss Prefab'ý atanmamýþ!");
        }
    }

    public void AddDestroyedBlock()
    {
        destroyedBlockCount++;

        // HER 4 BLOKTA BÝR +1 ROKET KAZANDIRMA MANTIÐI
        if (destroyedBlockCount % 4 == 0)
        {
            rocketCount++;
            UpdateRocketUI();
            Debug.Log("4 blok kýrýldý! +1 Roket kazandýn");
        }
    }

    public void UseRocket()
    {
        if (rocketCount > 0)
        {
            rocketCount--;
            UpdateRocketUI();
        }
    }

    void UpdateRocketUI()
    {
        if (rocketText != null) rocketText.text = "Roket: " + rocketCount;
    }

    void TriggerBuffSelection()
    {
        isBuffSelectionActive = true;
        selectionTimer = maxSelectionTime;
        Time.timeScale = 0f;
        if (buffSelectionPanel != null) buffSelectionPanel.SetActive(true);
    }

    public void OnBuffSelected(string buffType)
    {
        string lowerBuff = buffType.ToLower();
        PlayerShooting playerShooting = FindFirstObjectByType<PlayerShooting>();

        if (playerShooting != null)
        {
            if (lowerBuff == "speed") playerShooting.IncreaseSpeed();
            else if (lowerBuff == "count") playerShooting.IncreaseCount();
        }
        CloseBuffPanel();
    }

    void CloseBuffPanel()
    {
        Time.timeScale = 1f;
        isBuffSelectionActive = false;
        if (buffSelectionPanel != null) buffSelectionPanel.SetActive(false);
    }

    void UpdateLevelText()
    {
        if (levelText != null) levelText.text = "Level: " + currentDifficultyLevel;
    }

    public void TakeDamage()
    {
        playerHealth--;
        UpdateHealthUI();

        // Hasar aldýðýnda ekran titresin
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.TriggerShake(0.2f, 0.1f);
        }

        // Hasar aldýðýmýz an oyun 0.12 saniyeliðine dondurulsun
        StartCoroutine(DoHitStop(0.12f));

        if (playerHealth <= 0)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("FailScene");
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null) healthText.text = "HP : " + playerHealth;
    }
}