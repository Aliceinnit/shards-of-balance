using UnityEngine;

public class killenemy : MonoBehaviour
{
    public GameObject player;
    public Transform respawnpoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            player.transform.position = respawnpoint.position;
        }
    }
}
