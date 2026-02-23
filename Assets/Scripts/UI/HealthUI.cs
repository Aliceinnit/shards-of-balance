using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    public PlayerHealth playerHealth;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    void Start()
    {
        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();

            playerHealth.OnLivesEnabled += EnableLives;
    }
    void OnDestroy()
    {
        playerHealth.OnLivesEnabled -= EnableLives;
    }
    void EnableLives()
    {
         foreach (var heart in hearts)
                heart.enabled = false;
    }

    void Update()
    {
        if (playerHealth == null) 
        {
            Debug.LogError("Playerhealth null");
            return;
        }

        if (!playerHealth.livesEnabled)
        {
            foreach (var heart in hearts)
                heart.enabled = false;

            return;
        }

        int health = playerHealth.health;
        int maxHealth = playerHealth.maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < maxHealth;

            if (i < maxHealth)
                hearts[i].sprite = (i < health) ? fullHeart : emptyHeart;
        }
    }
}
