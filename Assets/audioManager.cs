using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("Music")]
    public AudioClip background;

    [Header("Sound Effects")]
    public AudioClip death;
    public AudioClip jump;
    public AudioClip wallTouch;
    public AudioClip portalIn;
    public AudioClip portalOut;
    public AudioClip footsteps;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // background music always playing
        if (background != null && musicSource != null)
        {
            musicSource.clip = background;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // one-shot sound effects (jump, star pickup, etc.)
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && SFXSource != null)
            SFXSource.PlayOneShot(clip);
    }

    // looped footsteps while walking
    public void StartFootsteps()
    {
        if (SFXSource == null || footsteps == null) return;

        if (SFXSource.clip != footsteps)
            SFXSource.clip = footsteps;

        if (!SFXSource.isPlaying)
        {
            SFXSource.loop = true;
            SFXSource.Play();
        }
    }

    public void StopFootsteps()
    {
        if (SFXSource == null) return;

        if (SFXSource.clip == footsteps)
        {
            SFXSource.Stop();
            SFXSource.loop = false;
            SFXSource.clip = null;
        }
    }
}