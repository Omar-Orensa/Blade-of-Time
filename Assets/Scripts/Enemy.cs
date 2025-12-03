using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 1;
    public GameObject herbPrefab;

    private SpriteRenderer sr;
    private Color originalColor;

    private bool isStunned = false;
    private float stunDuration = 0.5f;

    private Rigidbody2D rb;
    private SimpleEnemyPatrol patrolScript;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        originalColor = sr.color;

        
        patrolScript = GetComponent<SimpleEnemyPatrol>();
    }

    public virtual void TakeDamage(int amount)
    {
        health -= amount;

        StartCoroutine(FlashRed());
        StartCoroutine(Stun());

        if (health <= 0)
        {
            Die();

        }
    }

    IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        sr.color = originalColor;
    }

    IEnumerator Stun()
    {
        if (isStunned) yield break;
        isStunned = true;

        // Store velocity & freeze movement
        Vector2 storedVel = rb.linearVelocity;
        rb.linearVelocity = Vector2.zero;

        // Stop patrol movement
        if (patrolScript != null)
            patrolScript.enabled = false;

        // ⭐ START FLICKER EFFECT DURING STUN ⭐
        StartCoroutine(FlickerStun());

        // Wait for stun duration
        yield return new WaitForSeconds(stunDuration);

        // Restore AI
        if (patrolScript != null)
            patrolScript.enabled = true;

        // Restore velocity
        rb.linearVelocity = storedVel;

        isStunned = false;
    }

    IEnumerator FlickerStun()
    {
        float elapsed = 0f;
        float flickerSpeed = 0.05f; // how fast it flickers

        while (elapsed < stunDuration)
        {
            elapsed += flickerSpeed;

            // Toggle visibility
            sr.enabled = !sr.enabled;

            yield return new WaitForSeconds(flickerSpeed);
        }

        // Make sure enemy is visible afterwards
        sr.enabled = true;
    }



    void Die()
    {
        StartCoroutine(FadeAndDie());
    }

    IEnumerator FadeAndDie()
    {
        GetComponent<Collider2D>().enabled = false;
        rb.simulated = false;

        float t = 0;
        float duration = 0.8f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            yield return null;
        }
        if (herbPrefab != null)
            Instantiate(herbPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
