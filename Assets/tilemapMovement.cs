using UnityEngine;

public class tilemapMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float moveSpeed = 5;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3.left * moveSpeed) * Time.deltaTime;
    }
}
