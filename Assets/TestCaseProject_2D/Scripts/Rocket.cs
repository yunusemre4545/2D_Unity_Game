using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Patlama Ayarlarý")]
    public float explosionRadius = 2.5f; // Patlama çemberinin büyüklüðü
    public int explosionDamage = 4; // Çember içindeki bloklara ve Boss'a verilecek hasar

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
        // Patlama merkezindeki tüm nesneleri bul
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in colliders)
        {
            // Eðer patlama alanýnda normal blok varsa hasar ver
            DestructibleBlock block = hit.GetComponent<DestructibleBlock>();
            if (block != null)
            {
                block.TakeDamage(explosionDamage);
            }

            // Eðer patlama alanýnda Boss varsa ona da hasar ver
            BossController boss = hit.GetComponent<BossController>();
            if (boss != null)
            {
                boss.TakeDamage(explosionDamage);
            }
        }

        // Patlama iþlemi bitince roketi sahneden sil ki ekrandan çýkýp gitmesin
        Destroy(gameObject);
    }

    // Unity editöründe roketin patlama yarýçapýný kýrmýzý bir çizgiyle görmeni saðlar
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}