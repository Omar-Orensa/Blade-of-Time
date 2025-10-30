using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Portal : MonoBehaviour
{
    [SerializeField] string nextSceneName;
    [SerializeField] float vanishTime = 0.5f; // fade duration
    [SerializeField] Animator portalAnim;     // optional

    private bool used = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;
        if (!other.CompareTag("Player")) return;
        used = true;
        StartCoroutine(EnterPortal(other.gameObject));
    }

    private IEnumerator EnterPortal(GameObject player)
    {
        // Stop movement
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb) rb.linearVelocity = Vector2.zero;

        // Optional portal animation
        if (portalAnim) portalAnim.SetTrigger("Activate");

        // Center player on portal
        player.transform.position = transform.position;

        // Fade or shrink the player
        var sr = player.GetComponent<SpriteRenderer>();
        float t = 0;
        Color c = sr.color;

        while (t < vanishTime)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, t / vanishTime); // fade to transparent
            sr.color = new Color(c.r, c.g, c.b, a);

            // Optional shrink
            player.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t / vanishTime);
            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
