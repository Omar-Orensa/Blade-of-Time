using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] Button musicToggleButton;
    [SerializeField] TextMeshProUGUI musicButtonText;
    [SerializeField] Button backButton;

    // Optional: Reference to the parent menu (Pause or Main) to re-enable it
    [SerializeField] GameObject parentMenuToEnable;

    void Start()
    {
        // 1. Setup Music Button
        if (musicToggleButton != null)
        {
            musicToggleButton.onClick.AddListener(OnMusicToggleClicked);
        }

        // 2. Setup Back Button
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackClicked);
        }

        // 3. Initialize Text
        UpdateMusicButtonText();
    }

    void OnEnable()
    {
        // ⭐ CRITICAL: Sync button text with actual audio state when opening panel
        if (AudioManager.instance != null)
        {
            // Update button text to match current state
            UpdateMusicButtonText();
        }
    }

    public void OnMusicToggleClicked()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ToggleMusic();
            UpdateMusicButtonText();
        }
    }

    public void OnBackClicked()
    {
        // Close this panel
        gameObject.SetActive(false);

        // If we came from another menu (like Pause), re-enable it
        if (parentMenuToEnable != null)
        {
            parentMenuToEnable.SetActive(true);
        }
    }

    void UpdateMusicButtonText()
    {
        if (musicButtonText != null && AudioManager.instance != null)
        {
            // We check the actual AudioSource to see if it's muted
            bool isMuted = AudioManager.instance.GetComponent<AudioSource>().mute;
            musicButtonText.text = isMuted ? "MUSIC: OFF" : "MUSIC: ON";
        }
    }
}