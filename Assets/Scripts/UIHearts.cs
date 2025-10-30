using UnityEngine;
using UnityEngine.UI;

public class UIHearts : MonoBehaviour
{
    public static UIHearts Instance;
    [SerializeField] Image[] hearts; // assign Heart1..Heart3

    void Awake() { Instance = this; }

    public void UpdateHearts(int current, int max)
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].enabled = i < current;
    }
}
