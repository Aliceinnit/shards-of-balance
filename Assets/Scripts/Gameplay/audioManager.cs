using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxOneShotSource;
    [SerializeField] private AudioSource sfxLoopSource;

    [Header("Music")]
    public AudioClip background;

    [Header("Sound Effects")]
    public AudioClip death;
    public AudioClip jump;
    public AudioClip wallTouch;   // you used this for pickup
    public AudioClip portalIn;
    public AudioClip portalOut;
    public AudioClip footsteps;
    public AudioClip defeatVillan;
    public AudioClip winning;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (background != null && musicSource != null)
        {
            musicSource.clip = background;
            musicSource.loop = true;
            if (!musicSource.isPlaying)
                musicSource.Play();
        }
    }

    // Generic one-shot
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxOneShotSource == null) return;
        sfxOneShotSource.PlayOneShot(clip, volume);
    }

    // Convenience wrappers
    public void PlayDeath() => PlaySFX(death);
    public void PlayJump() => PlaySFX(jump);
    public void PlayPickup() => PlaySFX(wallTouch);
    public void PlayPortalIn() => PlaySFX(portalIn);
    public void PlayPortalOut() => PlaySFX(portalOut);

    // Loop footsteps (separate source)
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