using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    void Start()
    {
        // 1. Force-load health from GameManager
        GameManager gm = GameManager.instance;

        if (gm != null)
        {
            // Safety check: If GM says 0 but we just loaded, force it to full
            // (This handles the edge case where reset happened too late in the previous scene)
            if (gm.currentHearts <= 0)
            {
                gm.currentHearts = gm.maxHearts;
            }

            // 2. Update the UI immediately
            if (UIHearts.Instance != null)
            {
                UIHearts.Instance.UpdateHearts(gm.currentHearts, gm.maxHearts);
            }
        }
    }

    public void TakeDamage(int dmg = 1)
    {
        GameManager gm = GameManager.instance;
        if (gm == null) return;

        // Already dead? stop
        if (gm.currentHearts <= 0) return;

        // Apply damage
        gm.currentHearts -= dmg;

        // Update UI
        UIHearts.Instance.UpdateHearts(gm.currentHearts, gm.maxHearts);

        // Camera shake
        var cam = FindFirstObjectByType<CameraShake>();
        if (cam != null)
            cam.StartCoroutine(cam.Shake(0.25f, 0.2f));

        // Death case
        if (gm.currentHearts <= 0)
        {
            Debug.Log("Player dead");

            // ⭐ NEW CODE: Find the Game Over Manager and show the screen
            var gameOver = FindFirstObjectByType<GameOverManager>();
            if (gameOver != null)
            {
                gameOver.ShowGameOver();
            }
        }
    }

    // ⭐ FIXED HEAL METHOD (works with your GameManager system)
    public void Heal(int amount)
    {
        GameManager gm = GameManager.instance;
        if (gm == null) return;

        gm.currentHearts = Mathf.Min(gm.currentHearts + amount, gm.maxHearts);

        // Update UI
        UIHearts.Instance.UpdateHearts(gm.currentHearts, gm.maxHearts);
    }
}
