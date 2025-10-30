using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHearts = 3;
    int currentHearts;

    void Start()
    {
        currentHearts = maxHearts;
        UIHearts.Instance.UpdateHearts(currentHearts, maxHearts);
    }

    public void TakeDamage(int dmg = 1)
    {
        if (currentHearts <= 0) return;

        currentHearts -= dmg;
        UIHearts.Instance.UpdateHearts(currentHearts, maxHearts);

        FindFirstObjectByType<CameraShake>()?.StartCoroutine(
            FindFirstObjectByType<CameraShake>().Shake(0.25f, 0.2f));

        if (currentHearts <= 0)
        {
            // handle death here (e.g., respawn or reset)
            Debug.Log("Player dead");
        }
    }
}
