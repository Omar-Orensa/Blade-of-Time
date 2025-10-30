using UnityEngine;
public class DamageOnTouch : MonoBehaviour
{
    [SerializeField] int damage = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        Debug.Log("Player touched damage zone and took " + damage + " damage.");
    }
}
