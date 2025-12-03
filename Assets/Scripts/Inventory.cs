using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public int crystals;

    // ⭐ Removed [SerializeField] because we find it automatically now
    private TMP_Text crystalText;

    private GameObject portal;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ⭐ This runs every time a new level loads
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            // Optional: Hide specific inventory UI if it persists
            return;
        }
        // 1. Reset crystals for the new level
        crystals = 0;

        // 2. Find the NEW Text object in the new scene
        // IMPORTANT: Your UI Text must be named "CrystalCountText" in the Hierarchy
        GameObject textObj = GameObject.Find("CrystalCountText");

        if (textObj != null)
        {
            crystalText = textObj.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogError("Inventory: Could not find 'CrystalCountText' in this scene!");
        }

        // ⭐ ADD THIS BLOCK:
        if (GameManager.instance != null)
        {
            GameManager.instance.SaveLevelStartSnapshot();
        }

        // 3. Update the UI immediately so it shows "0/3"
        UpdateUI();

        // 4. Find the portal automatically
        portal = GameObject.Find("Portal");

        if (portal != null)
        {
            portal.SetActive(false); // Hide it immediately
        }
    }

    public void AddCrystal(int n)
    {
        crystals += n;
        UpdateUI();
        CheckPortal();
    }

    void UpdateUI()
    {
        // Only try to update if we found the text object
        if (crystalText != null)
        {
            crystalText.text = $"{crystals}/3";
        }
    }

    void CheckPortal()
    {
        if (crystals >= 3 && portal != null)
        {
            portal.SetActive(true);
        }
    }
}