using UnityEngine;
using UnityEngine.SceneManagement; 

public class CompleteUI : MonoBehaviour
{
    
    public void OnReturnMenuClicked()
    {
        
        Time.timeScale = 1f;


        SceneManager.LoadScene("StartScene");
    }
}