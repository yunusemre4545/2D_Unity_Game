using UnityEngine;

public class StartUI : MonoBehaviour
{
    // Butona verdiðimizde oyunu baþlatacak fonksiyon
    public void OnTapToStart()
    {
        GameManager.Instance.StartGame();
    }
}