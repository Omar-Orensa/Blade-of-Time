using UnityEngine;
using System.Collections;

public class PortalGrowAppear : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] float delayBeforeShrink = 0.5f;
    [Header("Appear Settings")]
    [SerializeField] float appearDuration = 0.7f;
    [SerializeField] Vector2 targetScale = new Vector2(14.579f, 10f);
    [SerializeField] float startScaleMultiplier = 0.1f;

    [Header("Disappear Settings")]
    [SerializeField] float disappearDuration = 0.5f;

    // ⭐ NEW: Portal Sound Effect
    [Header("Audio")]
    [SerializeField] AudioClip portalSound;

    public bool IsDisappearing { get; private set; } = false;

    void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(AppearEffect());
    }

    IEnumerator AppearEffect()
    {
        // ⭐ Play Sound on Appear
        if (AudioManager.instance != null && portalSound != null)
        {
            AudioManager.instance.PlaySFX(portalSound);
        }

        Vector3 startScale = new Vector3(
            targetScale.x * startScaleMultiplier,
            targetScale.y * startScaleMultiplier,
            1f
        );

        transform.localScale = startScale;

        Color c = sr.color;
        c.a = 0;
        sr.color = c;

        float t = 0f;

        while (t < appearDuration)
        {
            t += Time.deltaTime;
            float progress = t / appearDuration;

            float newX = Mathf.Lerp(startScale.x, targetScale.x, progress);
            float newY = Mathf.Lerp(startScale.y, targetScale.y, progress);
            transform.localScale = new Vector3(newX, newY, 1f);

            c.a = Mathf.Lerp(0f, 1f, progress);
            sr.color = c;

            yield return null;
        }

        transform.localScale = new Vector3(targetScale.x, targetScale.y, 1f);
        sr.color = new Color(c.r, c.g, c.b, 1f);
    }


    public IEnumerator DisappearEffect()
    {
        IsDisappearing = true;

        yield return new WaitForSeconds(delayBeforeShrink);

        Color c = sr.color;
        float t = 0f;

        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;

        while (t < disappearDuration)
        {
            t += Time.deltaTime;
            float p = t / disappearDuration;

            // Fade out
            c.a = Mathf.Lerp(1f, 0f, p);
            sr.color = c;

            // Shrink
            transform.localScale = Vector3.Lerp(startScale, endScale, p);

            yield return null;
        }

        transform.localScale = endScale;
    }
}