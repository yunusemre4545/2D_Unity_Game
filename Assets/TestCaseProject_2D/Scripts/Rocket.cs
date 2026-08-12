using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Patlama Ayarlarý")]
    public float explosionRadius = 2.5f; 
    public int explosionDamage = 4; 

    void OnTriggerEnter2D(Collider2D other)
    {
        // Roket bir bloða VEYA BOSS'A çarptýðý an patlasýn
        if (other.GetComponent<DestructibleBlock>() != null || other.GetComponent<BossController>() != null)
        {
            Explode();
        }
    }

    void Explode()
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in colliders)
        {
           
            DestructibleBlock block = hit.GetComponent<DestructibleBlock>();
            if (block != null)
            {
                block.TakeDamage(explosionDamage);
            }

            
            BossController boss = hit.GetComponent<BossController>();
            if (boss != null)
            {
                boss.TakeDamage(explosionDamage);
            }
        }

       
        Destroy(gameObject);
    }

   
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}