using UnityEngine;

public class ObjectPulse : MonoBehaviour
{
    [Header("Pulse Settings")]
    [Tooltip("How fast it pulses")]
    [SerializeField] float pulseSpeed = 3f;

    [Header("Glow/Alpha Settings")]
    [SerializeField] float minAlpha = 0.6f; // Dimmest brightness (0 = invisible)
    [SerializeField] float maxAlpha = 1.0f; // Brightest brightness (1 = full color)

    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr) originalColor = sr.color;
    }

    void Update()
    {
        if (sr != null)
        {
            // Calculate a sine wave value that moves between -1 and 1
            float sineValue = Mathf.Sin(Time.time * pulseSpeed);

            // Convert sine (-1 to 1) to a 0 to 1 range for Lerp
            float t = (sineValue + 1f) / 2f;

            // Lerp (blend) between the min and max alpha
            float newAlpha = Mathf.Lerp(minAlpha, maxAlpha, t);

            // Apply new alpha without touching Scale
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
        }
    }
}