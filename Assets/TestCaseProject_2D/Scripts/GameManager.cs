using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private const string LevelKey = "CurrentLevel";

    void Awake()
    {
        // Singleton pattern: Sahne geçiþlerinde silinmesin ve tek olsun
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Kayýtlý olan seviyeyi getirir 
    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(LevelKey, 1);
    }

    // Bölüm geçildiðinde seviyeyi 1 artýrýr ve kaydeder
    public void AdvanceLevel()
    {
        int nextLevel = GetCurrentLevel() + 1;
        PlayerPrefs.SetInt(LevelKey, nextLevel);
        PlayerPrefs.Save();
    }

    // Oyunu baþlatmak için Gameplay sahnesine geçiþ
    public void StartGame()
    {
        SceneManager.LoadScene("GamePlayScene");
    }

    // Bölüm bittiðinde veya ana menüye dönmek için
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void LoadCompleteScene()
    {
        SceneManager.LoadScene("CompleteScene");
    }
}