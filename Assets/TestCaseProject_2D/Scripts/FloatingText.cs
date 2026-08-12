using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 50f; 
    public float destroyTime = 1f;

    private TextMeshProUGUI textMesh; 
    private Color textColor;

    void Awake()
    {
        // UI Text bileþenini alýyoruz
        textMesh = GetComponent<TextMeshProUGUI>();
        if (textMesh != null)
        {
            textColor = textMesh.color;
        }
    }

    void Update()
    {
        
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        if (textMesh != null)
        {
            // Þeffaflaþma (Fade out) efekti
            textColor.a -= Time.deltaTime / destroyTime;
            textMesh.color = textColor;
        }

        // Süresi dolunca objeyi tamamen yok et
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string message, Color color)
    {
        if (textMesh != null)
        {
            textMesh.text = message;
            textMesh.color = color;
            textColor = color;
        }
    }
}