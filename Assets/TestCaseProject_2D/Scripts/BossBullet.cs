using UnityEngine;

public class BossBullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Eðer mermi sana (Player) çarparsa canýný azaltýr
        if (other.CompareTag("Player"))
        {
            if (GameplayManager.Instance != null)
            {
                GameplayManager.Instance.TakeDamage();
            }
            Destroy(gameObject); // Çarptýktan sonra mermi yok olsun
        }
        // Eðer mermi ekranýn en altýna (BottomTrigger) deðerse boþa gitmiþ demektir, sahneden silinsin
        else if (other.name.Contains("Bottom"))
        {
            Destroy(gameObject);
        }
    }
}