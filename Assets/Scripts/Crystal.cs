using UnityEngine;
public class Crystal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger with: " + other.name);
        if (!other.CompareTag("Player")) return;
        Object.FindFirstObjectByType<Inventory>().AddCrystal(1);
        Destroy(gameObject);
    }
}
