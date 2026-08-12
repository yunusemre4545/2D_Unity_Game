using UnityEngine;
using UnityEngine.SceneManagement;

public class FailUI : MonoBehaviour
{
   
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
}