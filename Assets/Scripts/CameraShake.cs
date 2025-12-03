using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration = 0.25f, float magnitude = 0.15f)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // ⭐ FIX: If the game freezes (Death/Pause), stop shaking immediately!
            if (Time.timeScale == 0f)
            {
                transform.localPosition = originalPos;
                yield break; // Exit the loop
            }

            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}