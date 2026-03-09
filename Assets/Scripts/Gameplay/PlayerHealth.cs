using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public bool livesEnabled = false;     
    public int maxHealth = 2;
    public int health;

    public Transform currentRespawnPoint;

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

    private bool tutorialFirstDeathHandled = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        playerMovement = GetComponent<Player>();
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
        health = maxHealth;
        Debug.Log("Start health = " + health + ", livesEnabled = " + livesEnabled);
        if (currentRespawnPoint == null)
            currentRespawnPoint = transform;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    if ( scene.name != "Crystal Forest")
        {
        health = maxHealth;
        livesEnabled = true;
            tutorialFirstDeathHandled = true;
        //Debug.LogWarning("Scene load refill: " + health + ", livesEnabled = " + livesEnabled + ", scene = " + scene.name);

        }
    else livesEnabled = false;


    if (currentRespawnPoint == null)
        currentRespawnPoint = transform;
}

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

    public void RespawnOnly()
    {
        if (invulnerable) return;
        StartCoroutine(RespawnRoutine(currentRespawnPoint));
    }

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

        int oldHealth = health;
        health = Mathf.Clamp(health - amount, 0, maxHealth);

        if (health < oldHealth && AudioManager.Instance != null)
        {
            AudioManager.Instance.StopFootsteps();
            AudioManager.Instance.PlayDeath();
        }

        if (health <= 0)
        {
            OnDied?.Invoke();

            if (!tutorialFirstDeathHandled)
            {
                tutorialFirstDeathHandled = true;
                return;
            }

            OnDeath();
        }
    }

    private void OnDeath()
    {
        if (playerMovement != null)
            playerMovement.enabled = false;

        var mainCam = Camera.main;
        if (mainCam != null)
            mainCam.gameObject.SetActive(false);

        if (deathCamera != null)
            deathCamera.SetActive(true);
    }

    public void RespawnNow()
    {
        health = maxHealth;

        if (playerMovement != null)
            playerMovement.enabled = true;

        var mainCam = Camera.main;
        if (mainCam != null)
            mainCam.gameObject.SetActive(true);

        if (deathCamera != null)
            deathCamera.SetActive(false);

        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(RespawnRoutine(currentRespawnPoint));
    }

    private IEnumerator RespawnRoutine(Transform point)
    {
        if (point == null) yield break;

        invulnerable = true;

        Vector2 target = (Vector2)point.position + Vector2.up * 0.2f;

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