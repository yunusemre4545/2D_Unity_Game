using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject blockPrefab;
    public float baseSpawnInterval = 1.5f;
    private float currentSpawnInterval;
    private float timer;

    void Update()
    {
        int currentLevel = 1;
        if (GameplayManager.Instance != null)
        {
            currentLevel = GameplayManager.Instance.currentDifficultyLevel;
        }

        currentSpawnInterval = baseSpawnInterval - ((currentLevel - 1) * 0.40f);

        if (currentSpawnInterval < 0.5f)
        {
            currentSpawnInterval = 0.5f;
        }

        timer += Time.deltaTime;
        if (timer >= currentSpawnInterval)
        {
            SpawnBlock();
            timer = 0f;
        }
    }

    void SpawnBlock()
    {
        float randomX = 0f;

        // Boss savaþýndaysak bloklar sadece sað veya sol kenarlardan düþsün
        if (GameplayManager.Instance != null && GameplayManager.Instance.isBossPhase)
        {
            // %50 ihtimalle sol kenar, %50 ihtimalle sað kenar
            if (Random.value > 0.5f)
            {
                randomX = Random.Range(-2.5f, -1.5f); 
            }
            else
            {
                randomX = Random.Range(1.5f, 2.5f); 
            }
        }
        else
        {
            
            randomX = Random.Range(-2.5f, 2.5f);
        }

        Vector3 spawnPos = new Vector3(randomX, 6f, 0f);
        GameObject newBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity);

        float randomScale = Random.Range(0.7f, 1.3f);
        newBlock.transform.localScale = new Vector3(randomScale, randomScale, 1f);
    }
}