using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için gerekli kütüphane

public class CompleteUI : MonoBehaviour
{
    // Ana menüye (StartScene) dönmek için fonksiyon
    public void OnReturnMenuClicked()
    {
        // Zamaný normale döndürelim (oyun içinde donuk kalmasýn)
        Time.timeScale = 1f;

        // Doðrudan ana menü sahnesinin adýný yazarak geçiþ yapýyoruz
        SceneManager.LoadScene("StartScene");
    }
}