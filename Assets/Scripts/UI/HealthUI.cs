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
            Debug.LogError("HeartsUI: Could not find PlayerHealth in scene.");
    }

    private void Update()
    {
        if (playerHealth == null) return;

        // Hide hearts before tutorial
        if (!playerHealth.livesEnabled)
        {
            for (int i = 0; i < hearts.Length; i++)
                if (hearts[i] != null) hearts[i].enabled = false;

            return;
        }

        int health = playerHealth.health;
        int maxHealth = playerHealth.maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null) continue;

            bool slotUsed = i < maxHealth;
            hearts[i].enabled = slotUsed;

            if (slotUsed)
                hearts[i].sprite = (i < health) ? fullHeart : emptyHeart;
        }
    }
}