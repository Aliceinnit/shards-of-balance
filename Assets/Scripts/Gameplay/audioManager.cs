using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxOneShotSource;
    [SerializeField] private AudioSource sfxLoopSource;

    [Header("Music")]
    public AudioClip level1Music;
    public AudioClip level2Music;

    [Header("Sound Effects")]
    public AudioClip death;
    public AudioClip jump;
    public AudioClip wallTouch;
    public AudioClip portalIn;
    public AudioClip portalOut;
    public AudioClip footsteps;
    public AudioClip defeatVillan;
    public AudioClip winning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        UpdateMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusicForScene(scene.name);
    }

    private void UpdateMusicForScene(string sceneName)
    {
        if (musicSource == null) return;

        AudioClip targetClip = null;

        if (sceneName == "Crystal Forest")
            targetClip = level1Music;
        else if (sceneName == "Skyruins")
            targetClip = level2Music;

        if (targetClip == null) return;

        if (musicSource.clip == targetClip && musicSource.isPlaying)
            return;

        musicSource.Stop();
        musicSource.clip = targetClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxOneShotSource == null) return;
        sfxOneShotSource.PlayOneShot(clip, volume);
    }

    public void PlayDeath() => PlaySFX(death);
    public void PlayJump() => PlaySFX(jump);
    public void PlayPickup() => PlaySFX(wallTouch);
    public void PlayPortalIn() => PlaySFX(portalIn);
    public void PlayPortalOut() => PlaySFX(portalOut);

    public void StartFootsteps()
    {
        if (sfxLoopSource == null || footsteps == null) return;

        if (sfxLoopSource.clip != footsteps)
            sfxLoopSource.clip = footsteps;

        sfxLoopSource.loop = true;

        if (!sfxLoopSource.isPlaying)
            sfxLoopSource.Play();
    }

    public void StopFootsteps()
    {
        if (sfxLoopSource == null) return;

        if (sfxLoopSource.clip == footsteps)
            sfxLoopSource.Stop();

        sfxLoopSource.loop = false;
        sfxLoopSource.clip = null;
    }
}