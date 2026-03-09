using UnityEngine;

public class Killenemy : MonoBehaviour
{
     public int damage = 1;

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log($"Killenemy TRIGGER: other={other.name}, tag={other.tag}, root={other.transform.root.name}");
        if (!other.CompareTag("Player")) return;
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
        // ph.RespawnOnly();
    }
}
