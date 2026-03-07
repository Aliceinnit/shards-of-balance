using Unity.VisualScripting;
using UnityEngine;

public class MonsterChase : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;

    void Update()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Monster collided with player!");
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth.TakeDamage(2);
        }
    }
}
