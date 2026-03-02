using UnityEngine;

public class Killenemy : MonoBehaviour
{
     public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var ph = other.GetComponentInParent<PlayerHealth>();
        if (ph == null) return;
        
        Debug.Log("trigger");
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
