using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject gameOverPanel;

    void Awake()
    {
        // Ensure the panel is hidden on load
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void RestartLevel()
    {
        // 1. Undo Collectibles
        if (GameManager.instance != null)
            GameManager.instance.DiscardLevelProgress();

        // 2. Unfreeze Time
        Time.timeScale = 1f;

        // 3. ⭐ RESET HEALTH HERE (Critical)
        if (GameManager.instance != null)
        {
            GameManager.instance.currentHearts = GameManager.instance.maxHearts;
        }

        // 4. Reload Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ⭐ UPDATED FUNCTION
    public void GoToMainMenu()
    {
        // 1. Save the level we just died in as the "Last Level"
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastLevel", currentScene);
        PlayerPrefs.Save();

        if (GameManager.instance != null) GameManager.instance.DiscardLevelProgress();

        // ⭐ Reset Health Logic ...
        if (GameManager.instance != null) GameManager.instance.currentHearts = GameManager.instance.maxHearts;

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}