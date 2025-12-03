using UnityEngine;

public class PickupHerb : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Heal Player
            PlayerHealth hp = other.GetComponent<PlayerHealth>();
            if (hp != null) hp.Heal(1);

            // 2. Add to Total Count
            if (GameManager.instance != null)
            {
                GameManager.instance.AddHerb();
            }

            Destroy(gameObject);
        }
    }
}