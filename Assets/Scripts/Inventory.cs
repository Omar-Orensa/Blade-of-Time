using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int crystals;
    [SerializeField] TMP_Text crystalText;

    void Start() { UpdateUI(); }

    public void AddCrystal(int n) { crystals += n; UpdateUI(); }

    void UpdateUI() { if (crystalText) crystalText.text = $"x{crystals}"; }
}
