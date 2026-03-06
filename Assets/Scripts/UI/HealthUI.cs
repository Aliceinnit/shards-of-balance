using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    public PlayerHealth playerHealth;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    private void Start()
    {
        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("HeartsUI: Could not find PlayerHealth in scene.");
            return;
        }

        UpdateHearts();
    }

    private void Update()
    {
        if (playerHealth == null) return;
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        // Hide hearts before tutorial is finished
        if (!playerHealth.livesEnabled)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (hearts[i] != null)
                    hearts[i].enabled = false;
            }
            return;
        }

        int health = playerHealth.health;
        int maxHealth = playerHealth.maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null) continue;

            // Show only as many heart slots as maxHealth
            hearts[i].enabled = i < maxHealth;

            if (i < maxHealth)
            {
                hearts[i].sprite = i < health ? fullHeart : emptyHeart;
            }
        }
    }
}