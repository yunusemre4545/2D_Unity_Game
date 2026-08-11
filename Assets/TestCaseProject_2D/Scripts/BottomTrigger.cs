using UnityEngine;

public class BottomTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Blok bu çizgiye deðdiði an yok olsun (Artýk barý artýrmýyoruz, bar süreye göre doluyor)
        if (other.name.Contains("Block"))
        {
            Destroy(other.gameObject);
        }
    }
}