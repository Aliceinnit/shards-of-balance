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
}
