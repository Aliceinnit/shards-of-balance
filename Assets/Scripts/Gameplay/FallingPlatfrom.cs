using UnityEngine;
using System.Collections;

public class FallingPlatfrom : MonoBehaviour
{
    float fallwait = 2f;
    float destroywait = 1f;

    bool isFalling;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void onCollisionEnter2D(Collision2D collision)
    {
        if(!isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallwait);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroywait);
    }
}
