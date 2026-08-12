using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 15f;

    void Update()
    {
       
        // Eðer buff seçme ekraný açýksa hareket kodlarýný çalýþtýrma ve direkt çýk!
        if (GameplayManager.Instance != null && GameplayManager.Instance.isBuffSelectionActive)
        {
            return;
        }

        Vector3 currentPos = transform.position;
        float targetX = currentPos.x;

        // Klavye Kontrolü 
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                targetX -= moveSpeed * Time.deltaTime;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                targetX += moveSpeed * Time.deltaTime;
        }

        // Fare Kontrolü 
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            targetX = Mathf.Lerp(currentPos.x, mousePos.x, moveSpeed * Time.deltaTime);
        }

        // Dokunmatik Ekran Kontrolü (Mobil / Telefon için)
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldTouchPos = Camera.main.ScreenToWorldPoint(touchPos);
            targetX = Mathf.Lerp(currentPos.x, worldTouchPos.x, moveSpeed * Time.deltaTime);
        }

        
        transform.position = new Vector3(targetX, currentPos.y, currentPos.z);

        
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        viewPos.x = Mathf.Clamp(viewPos.x, 0.05f, 0.95f);
        transform.position = Camera.main.ViewportToWorldPoint(viewPos);
    }
}