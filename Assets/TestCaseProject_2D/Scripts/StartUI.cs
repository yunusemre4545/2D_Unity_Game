using UnityEngine;

public class StartUI : MonoBehaviour
{
   
    public void OnTapToStart()
    {
        GameManager.Instance.StartGame();
    }
}