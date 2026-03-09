using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth ph = other.GetComponentInParent<PlayerHealth>();
        if (ph == null) return;

        // Before tutorial: just respawn
        if (!ph.livesEnabled)
        {
            ph.RespawnOnly();
            return;
        }

        // After tutorial: remove hearts
        ph.TakeDamage(damage);

        Player playerMovement = other.GetComponentInParent<Player>();
        if (playerMovement != null)
        {
            playerMovement.ApplyKnockback();
        }
    }
}