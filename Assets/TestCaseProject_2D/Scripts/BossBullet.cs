using UnityEngine;

public class BossBullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            if (GameplayManager.Instance != null)
            {
                GameplayManager.Instance.TakeDamage();
            }
            Destroy(gameObject);
        }
        
        else if (other.name.Contains("Bottom"))
        {
            Destroy(gameObject);
        }
    }
}