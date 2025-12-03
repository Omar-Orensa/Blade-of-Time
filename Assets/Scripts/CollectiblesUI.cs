using UnityEngine;
using TMPro;

public class CollectiblesUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panel; // Drag 'CollectiblesPanel' here

    [Header("Text Counters")]
    public TMP_Text redCount;    // Drag Slot_Red's text
    public TMP_Text greenCount;  // Drag Slot_Green's text
    public TMP_Text purpleCount; // Drag Slot_Purple's text
    public TMP_Text herbCount;   // Drag Slot_Herb's text

    void Update()
    {
        // Toggle when 'C' is pressed
        if (Input.GetKeyDown(KeyCode.C))
        {
            TogglePanel();
        }
    }

    public void TogglePanel()
    {
        bool isActive = !panel.activeSelf;
        panel.SetActive(isActive);

        if (isActive)
        {
            UpdateCounts();
        }
    }

    void UpdateCounts()
    {
        if (GameManager.instance == null) return;

        // Pull numbers from the GameManager
        if (redCount) redCount.text = GameManager.instance.totalRedCrystals.ToString();
        if (greenCount) greenCount.text = GameManager.instance.totalGreenCrystals.ToString();
        if (purpleCount) purpleCount.text = GameManager.instance.totalPurpleCrystals.ToString();
        if (herbCount) herbCount.text = GameManager.instance.totalHerbs.ToString();
    }
}