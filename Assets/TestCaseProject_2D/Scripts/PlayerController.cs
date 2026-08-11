using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 15f;

    void Update()
    {
        // --- GÜVENLÝK KÝLÝDÝ ---
        // Eðer buff seçme ekraný açýksa hareket kodlarýný çalýþtýrma ve direkt çýk!
        if (GameplayManager.Instance != null && GameplayManager.Instance.isBuffSelectionActive)
        {
            return;
        }

        Vector3 currentPos = transform.position;
        float targetX = currentPos.x;

        // Klavye Kontrolü (A-D / Yön Tuþlarý)
        if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            if (UnityEngine.InputSystem.Keyboard.current.aKey.isPressed || UnityEngine.InputSystem.Keyboard.current.leftArrowKey.isPressed)
                targetX -= moveSpeed * Time.deltaTime;
            if (UnityEngine.InputSystem.Keyboard.current.dKey.isPressed || UnityEngine.InputSystem.Keyboard.current.rightArrowKey.isPressed)
                targetX += moveSpeed * Time.deltaTime;
        }

        // Fare / Dokunmatik Sürükleme Kontrolü (Pürüzsüz Ývmeli Geçiþ)
        if (UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(UnityEngine.InputSystem.Mouse.current.position.ReadValue());

            // Hedef X pozisyonuna Lerp ile yumuþakça süzülerek git (15f hýz çarpanýdýr)
            targetX = Mathf.Lerp(currentPos.x, mousePos.x, moveSpeed * Time.deltaTime);
        }

        // Pozisyonu uygula
        transform.position = new Vector3(targetX, currentPos.y, currentPos.z);

        // --- TERS ÇEVRÝLMÝÞ VIEWPORT SINIRLANDIRMASI ---
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        // Sol kenar taþýyorsa 0.08 yerine büyütüyoruz, sað kenar taþýyorsa küçültüyoruz
        viewPos.x = Mathf.Clamp(viewPos.x, 0.05f, 0.95f);

        transform.position = Camera.main.ViewportToWorldPoint(viewPos);
    }
}