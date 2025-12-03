using UnityEngine;
using TMPro;
using System.Collections;

public class Guide : MonoBehaviour
{
    [TextArea]
    public string message; // the hint text for this scene

    public GameObject hintPopup;        // UI panel
    private TMP_Text hintText;          // Text inside the panel

    private Coroutine hideRoutine;      // For delayed fade
    private Coroutine typeRoutine;      // For typewriter effect

    void Start()
    {
        if (hintPopup != null)
        {
            hintText = hintPopup.GetComponentInChildren<TMP_Text>();
            hintPopup.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            ShowHint();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // If the object is already disabled/dying, don't run this logic
        if (!gameObject.activeInHierarchy) return;

        if (other.CompareTag("Player"))
            HideHintDelayed();
    }

    private void ShowHint()
    {
        if (hintPopup == null || hintText == null) return;

        // Cancel hiding if the player re-enters
        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        // Cancel typing if still typing
        if (typeRoutine != null)
            StopCoroutine(typeRoutine);

        // Reset text and show box
        hintPopup.SetActive(true);
        hintText.text = "";

        // Start the typewriter animation
        if (gameObject.activeInHierarchy)
        {
            typeRoutine = StartCoroutine(TypewriterEffect(message));
        }
    }

    IEnumerator TypewriterEffect(string msg)
    {
        foreach (char c in msg)
        {
            hintText.text += c;
            yield return new WaitForSeconds(0.03f);  // typing speed
        }
    }

    private void HideHintDelayed()
    {
        if (hintPopup == null) return;

        // ⭐ FIX: Check if active before starting coroutine
        if (gameObject.activeInHierarchy)
        {
            hideRoutine = StartCoroutine(FadeOutAfterDelay());
        }
        else
        {
            // If inactive, just close the popup instantly without animation
            hintPopup.SetActive(false);
        }
    }

    IEnumerator FadeOutAfterDelay()
    {
        // wait 2 seconds before fading
        yield return new WaitForSeconds(2f);

        CanvasGroup cg = hintPopup.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = hintPopup.AddComponent<CanvasGroup>();

        float t = 0;
        float duration = 1f; // fade-out duration

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1, 0, t / duration);
            yield return null;
        }

        hintPopup.SetActive(false);
        cg.alpha = 1; // reset for next time
    }
}