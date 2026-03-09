using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public int damage = 2;
    public float damageCooldown = 1f;

    float timer;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                PlayerHealth ph = other.GetComponent<PlayerHealth>();
                ph.TakeDamage(damage);
                timer = damageCooldown;
            }
        }
    }
}
