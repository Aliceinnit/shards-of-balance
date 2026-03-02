using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public bool livesEnabled = false; 
    public int maxHealth = 2;
    public int health;
    public GameObject deathCamera;
    public GameObject HeartsCount;

    public Transform currentRespawnPoint; // will change as player progresses

    public UnityAction OnLivesEnabled;

    private Rigidbody2D rb;

    private bool tutorialCompleted = false;

    public UnityAction OnDied;

    public float damageCooldown = 0.6f; // NEW: seconds
    private float nextDamageTime = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
       health = maxHealth;

        // safety fallback so respawn is never null
        if (currentRespawnPoint == null)
            currentRespawnPoint = transform;
    }

   public void SetRespawnPoint(Transform newPoint)
    {
        if (newPoint != null)
            currentRespawnPoint = newPoint;
    }

    // Use this BEFORE tutorial (gaps/plants just send you back)
    public void RespawnOnly()
    {
        RespawnAt(currentRespawnPoint);
    }

    // Use this AFTER tutorial (plants/gaps remove hearts)
    public void TakeDamage(int amount)
    {
        // If tutorial not done yet, don't use lives—just respawn
        if (!livesEnabled)
        {
            RespawnOnly();
            return;
        }

        if (amount <= 0) return;

        if (Time.time < nextDamageTime) return;
        nextDamageTime = Time.time + damageCooldown;

        health = Mathf.Clamp(health - amount, 0, maxHealth);

        if (health <= 0)
        {
            OnDied?.Invoke();

            // FIRST death during tutorial → wait for dialog
            if (!tutorialCompleted)
            {
                tutorialCompleted = true;
                return; // STOP here, don't respawn yet
            }

            // After tutorial → normal death
            Invoke("OnDeath", 0.5f);
        }
    }

    private void RespawnAt(Transform point)
    {
        if (point == null) return;

        Vector2 target = point.position;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;   // use velocity
            rb.angularVelocity = 0f;

            rb.position = target;         // teleport safely
        }
        else
        {
            transform.position = point.position;
        }
    }

    private void OnDeath()
    {
        if (tutorialCompleted)
        {
            GameObject playerCamera = GameObject.Find("Main Camera");
            GameObject player = GameObject.Find("bruh");
            Player playerMovements = player.GetComponent<Player>();
            playerMovements.enabled = false;
            playerCamera.SetActive(false);
            HeartsCount.SetActive(false);
            deathCamera.SetActive(true);
        }
    }

    public void RespawnNow()
    {
        RespawnAt(currentRespawnPoint);
        Time.timeScale = 1f;
        health = maxHealth;
    }
}
