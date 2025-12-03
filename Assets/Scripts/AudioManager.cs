using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;

    [Header("Level Music Tracks")]
    [Tooltip("Dojo: soft drums, bamboo flute")]
    public AudioClip dojoTheme;

    [Tooltip("Neon Edo: low synth pads, glitchy pulses")]
    public AudioClip neonTheme;

    [Tooltip("Desert: airy wind textures, distant chimes")]
    public AudioClip desertTheme;

    [Tooltip("Main Menu Theme")]
    public AudioClip menuTheme;

    // ⭐ NEW: Track Mute State
    private bool isMusicMuted = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip targetClip = null;

        switch (scene.name)
        {
            case "MainMenu":
                targetClip = menuTheme;
                break;
            case "Level0":
                targetClip = dojoTheme;
                break;
            case "Level1":
                targetClip = neonTheme;
                break;
            case "Level2":
                targetClip = desertTheme;
                break;
            default:
                return;
        }

        PlayMusic(targetClip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.Stop();
        musicSource.clip = clip;

        // ⭐ FORCE MUTE STATE
        musicSource.mute = isMusicMuted;

        // Always play, but rely on .mute to silence it
        musicSource.Play();
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxClip == null) return;

        GameObject sfxObj = new GameObject("SFX_" + sfxClip.name);
        AudioSource src = sfxObj.AddComponent<AudioSource>();
        src.clip = sfxClip;
        src.Play();
        Destroy(sfxObj, sfxClip.length);
    }

    // ⭐ NEW: Toggle Music Mute
    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;
        musicSource.mute = isMusicMuted;
    }
}