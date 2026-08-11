using UnityEngine;
using UnityEngine.SceneManagement;

public class FailUI : MonoBehaviour
{
    // Bu fonksiyon butona týklandýðýnda ana menüye dönecek
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("StartScene"); // StartScene ana menü sahnenin adý
    }
}