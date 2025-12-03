using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentHearts = 3;
    public int maxHearts = 3;

    public int totalRedCrystals = 0;
    public int totalGreenCrystals = 0;
    public int totalPurpleCrystals = 0;
    public int totalHerbs = 0;

    // ⭐ SNAPSHOT VARIABLES (To remember stats at start of level)
    private int startRed;
    private int startGreen;
    private int startPurple;
    private int startHerb;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // survive scene changes
        }
        else
        {
            Destroy(gameObject); // prevent duplicates
        }
    }

    // ⭐ Call this when a level finishes loading
    public void SaveLevelStartSnapshot()
    {
        startRed = totalRedCrystals;
        startGreen = totalGreenCrystals;
        startPurple = totalPurpleCrystals;
        startHerb = totalHerbs;
    }

    // ⭐ Call this if the player QUITS or RESTARTS mid-level
    public void DiscardLevelProgress()
    {
        // Revert totals back to what they were at the start
        totalRedCrystals = startRed;
        totalGreenCrystals = startGreen;
        totalPurpleCrystals = startPurple;
        totalHerbs = startHerb;
    }

    public void AddRedCrystal() { totalRedCrystals++; }
    public void AddGreenCrystal() { totalGreenCrystals++; }
    public void AddPurpleCrystal() { totalPurpleCrystals++; }
    public void AddHerb() { totalHerbs++; }

    // Link this to your "Main Menu" Button
    public void GoToMainMenu()
    {
        // 1. Unfreeze time (ALWAYS do this before loading scenes)
        Time.timeScale = 1f;

        // 2. Load the Menu Scene
        SceneManager.LoadScene("MainMenu");
    }
}