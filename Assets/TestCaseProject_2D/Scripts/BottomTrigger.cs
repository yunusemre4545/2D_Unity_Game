using UnityEngine;

public class BottomTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Blok bu çizgiye deðdiði an yok olsun 
        if (other.name.Contains("Block"))
        {
            Destroy(other.gameObject);
        }
    }
}