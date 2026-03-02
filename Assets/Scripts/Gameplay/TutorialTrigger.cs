using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Header("Tutorial Setup")]
    public Transform tutorialRespawnPoint;
    public GameObject plantToDisable; // drag the plant ROOT or the hurtbox object here
    public GameObject dialogBoxToEnable;

    private bool triggeredOnce = false;
    private PlayerHealth subscribedPH;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggeredOnce) return;
        if (!other.TryGetComponent<PlayerHealth>(out var ph)) return;

        triggeredOnce = true;

        // Enable hearts/lives system
        ph.livesEnabled = true;

        // Restore hearts right away
        ph.health = ph.maxHealth;

        // Set respawn point
        if (tutorialRespawnPoint != null)
            ph.SetRespawnPoint(tutorialRespawnPoint);

        // Listen for death so we can disable the plant when the player dies
        subscribedPH = ph;
        subscribedPH.OnDied -= HandlePlayerDied;
        subscribedPH.OnDied += HandlePlayerDied;

        // NEW: disable only the collider (prevents push/glitch from re-entering)
        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Optional: also hide/remove the trigger object
        // gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        // NEW: safety cleanup if object gets disabled/destroyed
        if (subscribedPH != null)
            subscribedPH.OnDied -= HandlePlayerDied;
    }

    private void HandlePlayerDied()
    {
        if (subscribedPH != null)
            subscribedPH.OnDied -= HandlePlayerDied;

        dialogBoxToEnable.SetActive(true);

        if (plantToDisable != null)
        Destroy(plantToDisable); 
    }
}