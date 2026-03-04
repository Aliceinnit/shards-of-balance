using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    
    public int damage = 1;
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log($"{name} TRIGGERED by: {other.name} (root: {other.transform.root.name})");
        var ph = other.GetComponentInParent<PlayerHealth>();
        if (ph == null) return;

        // Before tutorial: just respawn
        if (!ph.livesEnabled)
        {
            ph.RespawnOnly();
            return;
        }

        // After tutorial: remove hearts
        ph.TakeDamage(damage);
    }
}
