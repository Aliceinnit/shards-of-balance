using UnityEngine;

public class PlantKnockback : MonoBehaviour
{
    public float knockbackForce = 10f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Vector2 sourcePosition)
    {
        Vector2 direction = ((Vector2)transform.position - sourcePosition).normalized;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }
}
