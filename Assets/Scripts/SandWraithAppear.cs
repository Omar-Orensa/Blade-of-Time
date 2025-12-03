using UnityEngine;
using System.Collections;

public class SandWraithAppear : MonoBehaviour
{
    public Vector2 targetScale = new Vector2(0.3300453f, 0.29023f);
    public float appearDuration = 0.7f;
    public float delayBeforeMove = 1f;

    public Vector2 appearPosition = new Vector2(0.62f, -4.7f);

    private MonoBehaviour movementScript;
    private Vector3 hiddenScale = Vector3.zero;

    private bool hasSpawnedOnce = false;   // ⭐ prevents repeated spawning

    void Start()
    {
        movementScript = GetComponent<MonoBehaviour>();
        if (movementScript != null)
            movementScript.enabled = false;

        transform.localScale = hiddenScale;
    }

    public void Appear()
    {
        // ⭐ Do nothing if already spawned once
        if (hasSpawnedOnce)
            return;

        hasSpawnedOnce = true;
        StartCoroutine(AppearRoutine());
    }

    IEnumerator AppearRoutine()
    {
        transform.position = appearPosition;

        float t = 0f;
        Vector3 finalScale = new Vector3(targetScale.x, targetScale.y, 1f);

        while (t < appearDuration)
        {
            t += Time.deltaTime;
            float p = t / appearDuration;

            transform.localScale = Vector3.Lerp(hiddenScale, finalScale, p);
            yield return null;
        }

        transform.localScale = finalScale;

        yield return new WaitForSeconds(delayBeforeMove);

        if (movementScript != null)
            movementScript.enabled = true;
    }
}
