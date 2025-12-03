using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject pausePanel;    // The panel with Resume/Restart/etc.
    [SerializeField] GameObject settingsPanel; // The panel with Music/Back

    private bool isPaused = false;

    void Update()
    {
        // Allow unpausing with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        if (pausePanel != null) pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true); // Show Settings

            // ⭐ NEW: Hide the Pause Buttons
            if (pausePanel != null)
                pausePanel.SetActive(false);
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false); // Hide Settings
        }

        // ⭐ NEW: Bring back the Pause Buttons
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.DiscardLevelProgress();
            GameManager.instance.currentHearts = GameManager.instance.maxHearts;
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastLevel", currentScene);
        PlayerPrefs.Save();

        if (GameManager.instance != null)
        {
            GameManager.instance.DiscardLevelProgress();
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}