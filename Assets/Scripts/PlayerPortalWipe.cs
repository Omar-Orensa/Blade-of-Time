using UnityEngine;
using System.Collections;

public class PlayerPortalWipe : MonoBehaviour
{
    public Transform maskTransform;         // PlayerMask transform
    public float wipeDuration = 0.4f;       // How fast the wipe happens

    private Vector3 originalScale;

    void Start()
    {
        if (maskTransform != null)
            originalScale = maskTransform.localScale;
    }

    public IEnumerator WipeRightToLeft()
    {
        float t = 0f;
        Vector3 start = originalScale;
        Vector3 end = new Vector3(0f, originalScale.y, originalScale.z);

        while (t < wipeDuration)
        {
            t += Time.deltaTime;
            float p = t / wipeDuration;

            // shrink in X direction only (right → left wipe)
            maskTransform.localScale = Vector3.Lerp(start, end, p);

            yield return null;
        }

        maskTransform.localScale = end;
    }
}
