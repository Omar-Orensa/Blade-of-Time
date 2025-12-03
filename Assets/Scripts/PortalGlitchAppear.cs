using UnityEngine;
using System.Collections;

public class PortalGlitchAppear : MonoBehaviour
{
    private SpriteRenderer sr;

    void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(AppearEffect());
    }

    IEnumerator AppearEffect()
    {
        Vector3 originalPos = transform.position;

        // --- GLITCH PHASE (0.2s) ---
        for (int i = 0; i < 10; i++)
        {
            transform.position = originalPos + (Vector3)Random.insideUnitCircle * 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        // reset position
        transform.position = originalPos;

        // --- FADE IN PHASE ---
        Color c = sr.color;
        c.a = 0;
        sr.color = c;

        while (c.a < 1f)
        {
            c.a += Time.deltaTime * 1.0f;   // fade speed
            sr.color = c;
            yield return null;
        }
    }
}
