using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Portal : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField] string nextSceneName;

    [Tooltip("The story text shown on the loading screen when entering this portal.")]
    [TextArea]
    public string narrativeMessage = "Lost timelines drift beneath shifting sands…"; // Default text

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
        // 1. Stop movement
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb) rb.linearVelocity = Vector2.zero;

        // 2. Start Player Fade (Run in parallel)
        StartCoroutine(FadePlayer(player));

        // 3. Handle Portal Shrink Safely
        var portalAppearScript = GetComponent<PortalGrowAppear>();
        if (portalAppearScript != null && !portalAppearScript.IsDisappearing)
        {
            // Wait for it to shrink
            yield return StartCoroutine(portalAppearScript.DisappearEffect());
        }
        else
        {
            // If script is missing, just wait a small amount of time so it doesn't feel instant
            yield return new WaitForSeconds(0.5f);
        }

        // If the next scene is "MainMenu", we treat this as "Game Completed"
        if (nextSceneName == "MainMenu")
        {
            // Clear the "LastLevel" save so the Continue button becomes unclickable
            PlayerPrefs.DeleteKey("LastLevel");

            // Optional: Also reset collectibles if you want a full wipe
            if (GameManager.instance != null) GameManager.instance.DiscardLevelProgress();
        }
        else
        {
            // Normal Level Transition: Save the next level as our "Continue" point
            PlayerPrefs.SetString("LastLevel", nextSceneName);
        }
        PlayerPrefs.Save();

        // 4. Trigger Loading Screen with CUSTOM Message
        if (LoadingScreen.Instance != null)
        {
            LoadingScreen.Instance.LoadScene(
                nextSceneName,
                narrativeMessage // <--- NOW USES YOUR INSPECTOR VARIABLE
            );
        }
        else
        {
            Debug.LogError("LoadingScreen Instance is NULL! Make sure the Prefab is in the scene.");
            // Emergency fallback
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private IEnumerator FadePlayer(GameObject player)
    {
        var sr = player.GetComponent<SpriteRenderer>();
        float t = 0f;
        Color c = sr.color;

        Vector3 startScale = player.transform.localScale;

        while (t < vanishTime)
        {
            t += Time.deltaTime;
            float p = t / vanishTime;

            // Fade player instantly & fast
            float a = Mathf.Lerp(1f, 0f, p);
            sr.color = new Color(c.r, c.g, c.b, a);

            // Shrink player
            player.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, p);

            yield return null;
        }
    }
}