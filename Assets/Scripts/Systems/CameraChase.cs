using UnityEngine;

public class CameraChase : MonoBehaviour
{
    public Transform player;
    public Transform monster;
    public float cameraSpeed = 2f;

    void Update()
    {
        float cameraCenterX = transform.position.x;
        float playerX = player.position.x;
        float monsterX = monster.position.x;

        if (playerX > cameraCenterX - 5f)
        {
            transform.position += new Vector3(cameraSpeed * Time.deltaTime, 0, 0);
        }
    }
}
