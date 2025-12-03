using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;

    public GameObject loadingCanvas;
    public Image blackFade;
    public Image background;
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI narrativeText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            loadingCanvas.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName, string narrativeLine)
    {
        StartCoroutine(LoadRoutine(sceneName, narrativeLine));
    }

    IEnumerator LoadRoutine(string sceneName, string narrativeLine)
    {
        Debug.Log("Loading Screen Activated!");

        loadingCanvas.SetActive(true);

        // ⭐ FIX 1: Hide Art initially so we can see the Black Fade
        background.enabled = false;
        loadingText.gameObject.SetActive(false);
        narrativeText.gameObject.SetActive(false); // Ensure this is off too

        // 1. Fade game to black
        yield return StartCoroutine(FadeIn());

        // 2. NOW show the Loading Art & Loading Text (Pop in over black)
        background.enabled = true;
        loadingText.gameObject.SetActive(true);
        loadingText.alpha = 1;

        // 3. Setup Narrative Text
        narrativeText.text = narrativeLine;
        narrativeText.alpha = 0;
        narrativeText.gameObject.SetActive(true);

        // 4. Start Narrative Fade (Runs in parallel)
        StartCoroutine(FadeNarrative());

        // ⭐ FIX 2: Wait longer (3 Seconds)
        yield return new WaitForSeconds(3.0f);

        // 5. Load next scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 6. Fade back into gameplay
        yield return StartCoroutine(FadeOut());

        HideAllUI();
    }

    IEnumerator FadeIn()
    {
        // Fade TO Black
        for (float a = 0; a <= 1; a += Time.deltaTime * 1.5f) // Faster fade to black (optional)
        {
            blackFade.color = new Color(0, 0, 0, a);
            yield return null;
        }
        blackFade.color = new Color(0, 0, 0, 1); // Ensure fully black
    }

    IEnumerator FadeOut()
    {
        // Fade FROM Black
        for (float a = 1; a >= 0; a -= Time.deltaTime * 0.7f)
        {
            blackFade.color = new Color(0, 0, 0, a);
            yield return null;
        }
        blackFade.color = new Color(0, 0, 0, 0);
    }

    IEnumerator FadeNarrative()
    {
        // ⭐ FIX 3: Slower Text Fade (0.3f instead of 0.7f)
        float fadeSpeed = 0.4f;

        // Fade in
        for (float a = 0; a <= 1; a += Time.deltaTime * fadeSpeed)
        {
            narrativeText.alpha = a;
            yield return null;
        }

        // Hold text for a moment
        yield return new WaitForSeconds(1.0f);

        // Fade out
        for (float a = 1; a >= 0; a -= Time.deltaTime * fadeSpeed)
        {
            narrativeText.alpha = a;
            yield return null;
        }
    }

    private void HideAllUI()
    {
        loadingCanvas.SetActive(false);
        loadingText.gameObject.SetActive(false);
        narrativeText.gameObject.SetActive(false);
        background.enabled = false;
        blackFade.color = new Color(0, 0, 0, 0);
    }
}