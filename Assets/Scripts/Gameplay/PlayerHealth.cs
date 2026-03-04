using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public bool livesEnabled = false;     // set true when tutorial is completed
    public int maxHealth = 2;
    public int health;

    public Transform currentRespawnPoint; // gets updated as player progresses

    public GameObject deathCamera;

    public float damageCooldown = 0.6f;
    private float nextDamageTime = 0f;

    public float respawnInvulnTime = 0.25f;
    public bool invulnerable { get; private set; }

    public UnityAction OnLivesEnabled;
    public UnityAction OnDied;

    private Rigidbody2D rb;
    private Collider2D col;
    private Player playerMovement;

    // Tutorial special case: first death after lives become enabled
    private bool tutorialFirstDeathHandled = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        playerMovement = GetComponent<Player>();
    }

    private void Start()
    {
        health = maxHealth;

        if (currentRespawnPoint == null)
            currentRespawnPoint = transform;
    }

    // Called this when tutorial ends 
    public void EnableLivesSystem()
    {
        livesEnabled = true;
        health = maxHealth;

        tutorialFirstDeathHandled = false;

        OnLivesEnabled?.Invoke();
    }

    public void SetRespawnPoint(Transform newPoint)
    {
        if (newPoint != null)
            currentRespawnPoint = newPoint;
    }

    // before tutorial: gaps/plants just send you back
    public void RespawnOnly()
    {
        if (invulnerable) return;
        StartCoroutine(RespawnRoutine(currentRespawnPoint));
    }

    // after tutorial: hazards remove hearts
    public void TakeDamage(int amount)
    {
        if (!livesEnabled)
        {
            RespawnOnly();
            return;
        }

        if (amount <= 0) return;
        if (invulnerable) return;

        if (Time.time < nextDamageTime) return;
        nextDamageTime = Time.time + damageCooldown;

        health = Mathf.Clamp(health - amount, 0, maxHealth);

        if (health <= 0)
        {
            OnDied?.Invoke();

            // first death during tutorial phase after enabling lives
            if (!tutorialFirstDeathHandled)
            {
                tutorialFirstDeathHandled = true;
                return;
            }

            // After tutorial: normal death
            OnDeath();
        }
    }

    private void OnDeath()
    {
        // Stop player controls
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Switch cameras (optional)
        var mainCam = Camera.main;
        if (mainCam != null)
            mainCam.gameObject.SetActive(false);

        if (deathCamera != null)
            deathCamera.SetActive(true);
    }
    public void RespawnNow()
    {
        // Restore health
        health = maxHealth;

        // Restore player
        if (playerMovement != null)
            playerMovement.enabled = true;

        var mainCam = Camera.main;
        if (mainCam != null)
            mainCam.gameObject.SetActive(true);

        if (deathCamera != null)
            deathCamera.SetActive(false);

        // Teleport + invulnerability window
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(RespawnRoutine(currentRespawnPoint));
    }

    private IEnumerator RespawnRoutine(Transform point)
{
    if (point == null) yield break;

    invulnerable = true;

    Vector2 target = (Vector2)point.position + Vector2.up * 0.2f;

    // Reset physics
    if (rb != null)
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.position = target;
    }
    else
    {
        transform.position = target;
    }

    yield return new WaitForSeconds(respawnInvulnTime);

    invulnerable = false;
}
}