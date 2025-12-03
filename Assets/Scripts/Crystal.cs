using UnityEngine;

public enum CrystalColor { Red, Green, Purple }

public class Crystal : MonoBehaviour
{
    public CrystalColor colorType;

    [Header("Audio")]
    [SerializeField] AudioClip collectSound; // ⭐ New variable for the sound effect

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 1. Play Sound Effect (Before destroying object)
        if (AudioManager.instance != null && collectSound != null)
        {
            AudioManager.instance.PlaySFX(collectSound);
        }

        // 2. Add to Level Inventory
        Inventory inv = Object.FindFirstObjectByType<Inventory>();
        if (inv != null) inv.AddCrystal(1);

        // 3. Add to Total Collectibles
        if (GameManager.instance != null)
        {
            switch (colorType)
            {
                case CrystalColor.Red: GameManager.instance.AddRedCrystal(); break;
                case CrystalColor.Green: GameManager.instance.AddGreenCrystal(); break;
                case CrystalColor.Purple: GameManager.instance.AddPurpleCrystal(); break;
            }
        }

        Destroy(gameObject);
    }
}