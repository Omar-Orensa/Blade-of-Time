using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Include TMP namespace

public class MainMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Image backgroundImage;
    [SerializeField] Button continueButton;
    [SerializeField] GameObject settingsPanel;

    // ⭐ NEW: Music Toggle Button Reference
    [SerializeField] Button musicToggleButton;
    [SerializeField] TextMeshProUGUI musicButtonText; // Text on the button to show ON/OFF state

    [Header("Backgrounds")]
    [SerializeField] Sprite dojoSilhouette;
    [SerializeField] Sprite neonSkyline;
    [SerializeField] Sprite desertDunes;

    [Header("Audio")]
    [SerializeField] AudioClip swordSound;

    private string lastLevel;

    void Start()
    {
        lastLevel = PlayerPrefs.GetString("LastLevel", "");
        continueButton.interactable = !string.IsNullOrEmpty(lastLevel);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        UpdateBackgroundVisuals();

        // ⭐ Update button text on start based on current mute state
        UpdateMusicButtonText();
    }

    void UpdateBackgroundVisuals()
    {
        if (backgroundImage == null) return;

        switch (lastLevel)
        {
            case "Level2_NeonEdo":
                backgroundImage.sprite = neonSkyline;
                break;
            case "Level3_Desert":
                backgroundImage.sprite = desertDunes;
                break;
            default:
                backgroundImage.sprite = dojoSilhouette;
                break;
        }
    }

    public void OnStartClicked()
    {
        if (LoadingScreen.Instance != null)
            LoadingScreen.Instance.LoadScene("Level0", "The timeline fractures...");
        else
            SceneManager.LoadScene("Level0");
    }

    public void OnContinueClicked()
    {  

        if (!string.IsNullOrEmpty(lastLevel))
        {
            if (LoadingScreen.Instance != null)
                LoadingScreen.Instance.LoadScene(lastLevel, "Returning to the fracture...");
            else
                SceneManager.LoadScene(lastLevel);
        }
    }

    public void OnSettingsClicked()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    public void OnCloseSettingsClicked()
    {

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void OnQuitClicked()
    {
        Debug.Log("Quitting Game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // ⭐ NEW: Called when Music Button is clicked
    public void OnMusicToggleClicked()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ToggleMusic();
            UpdateMusicButtonText();
        }
    }

    // ⭐ Helper to update button text
    void UpdateMusicButtonText()
    {
        if (musicButtonText != null && AudioManager.instance != null)
        {
            bool isMuted = AudioManager.instance.GetComponent<AudioSource>().mute;
            musicButtonText.text = isMuted ? "MUSIC: OFF" : "MUSIC: ON";
        }
    }
}