using UnityEngine;
using TMPro; // Important: This lets us talk to TextMeshPro

public class BlinkingText : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float fadeSpeed = 1.5f;

    void Update()
    {
        // This creates a value that goes up and down between 0 and 1
        // Mathf.Sin creates a wave between -1 and 1
        // We add 1 and divide by 2 to force it into a 0 to 1 range
        float alphaValue = (Mathf.Sin(Time.time * fadeSpeed) + 1.0f) / 2.0f;

        // Apply the new alpha to the text
        textComponent.alpha = alphaValue;
    }
}